
const adminId ="google-oauth2|102081440954539974460"
import { PrismaClient } from '@prisma/client'
import { readFileSync, readdirSync } from 'fs'
import slugify from 'slugify'

const prisma = new PrismaClient({
    log: ['query', 'info', 'warn']
})

const createTagIfNotExists = async (tag) => {
    const tagExists = await prisma.tags.findUnique({
        where: {
            Name: tag.trim()
        }
    })
    if (tagExists) {
        return tagExists
    }
    return await prisma.tags.create({
        data: {
            Name: tag.trim(),
            Slug: slugify(tag.trim()),
        }
    })
}


async function main(fileName) {
    console.log(fileName)
    const data = JSON.parse(readFileSync(fileName));
    console.log(data.length)
    for (const article of data) {
        const { title,  tags, published_at, markdownContent, shortDescription, htmlMeta,displayImage, slug } = article;
        const existingArticle = await prisma.articles.findUnique({
            where: {
                Slug: slugify(slug,{strict:true,lower:true})
            }
        })
        if (existingArticle) {
            console.log(`Article with slug ${slug} already exists`)
            continue
        }
        let articleTags = tags.map(async (tag) => {
            const tagExists = await createTagIfNotExists(tag)
            return tagExists
        })
         articleTags = await Promise.all(articleTags)

        const newArticle = await prisma.articles.create({
            data:{
                    Content:markdownContent,
                    Created : new Date(published_at),
                    DisplayImage:displayImage,
                    LastModified:new Date(published_at),
                    Slug:slugify(slug,{strict:true,lower:true}),
                    CreatedBy:adminId,
                    LastModifiedBy:adminId,
                    ShortDescription:shortDescription,
                    Title:title,
            }
        })
        console.log(newArticle.Slug)
       await prisma.articleTags.createMany({
            data:articleTags.map((tag) => {
                return {
                    ArticlesId:newArticle.Id,
                    TagsId:tag.Id
                }
            })
        })
    } 


}
// data.typescript.json
const listFiles =readdirSync("./").filter((fileName) => fileName.endsWith(".json") && fileName.startsWith("data"))

for(const fileName of listFiles)
{
   main(fileName).then(() => {
         console.log("Done")
    }).catch((err) => {
        console.log(err)
    })
}