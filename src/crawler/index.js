import axios from 'axios';
import { load } from 'cheerio';
import slugify from 'slugify';
import { writeFileSync } from 'fs';

import TurndownService from 'turndown';
const turndownService = new TurndownService({
    headingStyle: 'atx',
    codeBlockStyle: 'fenced',
});

turndownService.addRule('highlight', {
    filter: ['pre'],
    replacement: function (content, node, options) {
        const language = node.className.replace('highlight', '').trim();
        return `\n\`\`\`${language}\n${content}\n\`\`\`\n`;
    }
});
const getTopWeekURL=(tag)=>
{

    const today = new Date();
    const lastWeek = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 30);
    const lastWeekString = lastWeek.toISOString();
    return `https://dev.to/search/feed_content?per_page=10&page=0&tag=${tag}&sort_by=public_reactions_count&sort_direction=desc&tag_names%5B%5D=${tag}&approved=&class_name=Article&published_at%5Bgte%5D=${lastWeekString}`
}

async function getData(url)
{
    const response = await axios.get(url);
    return response.data.result.map((article)=>({
        title:article.title,
        url:`https://dev.to${article.path}`,
        tags:article.tag_list,
        published_at:Number(article.published_at_int)*1000,
    }))
}

function createAuthorMdBlock({name,link})
{
    return `\n**Author**: [${name}](${link})`;
}

async function getDetails(articleBrief)
{
  const response = await axios.get(articleBrief.url);
    const html = response.data;
    const $ = load(html);
    const articleHtmlContent = $('.crayons-article__body.text-styles.spec__body').html();
    // author name selector: a.crayons-link.fw-bold
    const authorElement = $('a.crayons-link.fw-bold');
    const authorName = authorElement.text();
    const authorLink = `https://dev.to${authorElement.attr('href')}`;
    const authorBlock = createAuthorMdBlock({name:authorName,link:authorLink});
    const markdownContent = turndownService.turndown(articleHtmlContent);
    const markdownContentWithAuthor = `${markdownContent}\n${authorBlock}`;
    //crayons-article__cover > img
    //https://res.cloudinary.com/practicaldev/image/fetch/s--ehpvgY4---/c_imagga_scale,f_auto,fl_progressive,h_420,q_auto,w_1000/https://static-assets.amplication.com/blog/extending-gitops-effortless-continuous-integration-and-deployment-on-kubernetes/hero.png
    const displayImageRaw= $('.crayons-article__cover > img').attr('src');
    console.log(articleBrief.url,displayImageRaw);
   // locate last index of https:// and ignore the text before that
    const displayImage = displayImageRaw.substring(displayImageRaw.lastIndexOf('https://'));
    const htmlMeta = [
       $.html( $('meta[property="og:title"]').first()),
       $.html( $('meta[property="og:description"]').first()),
       $.html( $('meta[name="keywords"]').first()),
       $.html( $('meta[property="og:image"]').first()),
       $.html($('meta[property="og:type"]').first()),
    ].join('\n');
    return {
        ...articleBrief,
        markdownContent:markdownContentWithAuthor,
        shortDescription:$('meta[property="og:description"]').attr('content'),
        htmlMeta ,
        slug:slugify(articleBrief.title),
        displayImage
        
    };

}
async function main()
{
    const tag = 'dotnet';
    const url = getTopWeekURL(tag);
    const results =[]
    let articles = await getData(url); // only 2 articles
    // articles = articles.slice(0,2);
    for(let i=0;i<articles.length;i++)
    {
        let article = articles[i];
        try{
            article = await getDetails(article);
        results.push(article);

        }catch(e)
        {
        }
        
    }
    writeFileSync('data.'+tag+'.json',JSON.stringify(results,null,2));
}

main();
