import { Image,Divider, Chip} from "@nextui-org/react";
import { ArticleDto, Client } from "@/core/api_client";
import { createClient } from "@/core/client";
import { redirect } from "next/navigation";
import markdownit from "markdown-it";
import hljs from 'highlight.js'
import 'highlight.js/styles/github-dark.min.css';
import NextLink from "next/link";

interface ArticleCardProps {
  params: {
    slug: string;
  };
}

export default async function Slug({ params }: ArticleCardProps) {
  let article: ArticleDto;
  const md = markdownit({
    highlight(str, lang, attrs) {
      if (lang && hljs.getLanguage(lang)) {
        try {
          return hljs.highlight(str, { language: lang }).value;
        } catch (__) {}
      }
  
      return ''
    }
  });
  

  try {
    article = await createClient({}).getArticleByUniqueIdentifier(params.slug);
  } catch (e) {
    redirect("/404");
  }
  const tags = article?.tags || []
  return (
   <div className="w-full gap-y-1 flex-col flex  mx-auto xl:max-w-5xl">
     <article className="flex !max-w-full prose dark:prose-invert lg:prose-xl flex-col items-center gap-4 w-full ">
      <div className="flex flex-col justify-start w-full">
        <Image
          height={400}
          className="mb-unit-5  mx-auto object-cover"
          src={article!.displayImage}
          alt={article!.title}
        />

        <h1 className="">{article!.title}</h1>
      </div>
      <div
       className="max-w-full"
        dangerouslySetInnerHTML={{ __html: md.render(article!.content!) }}
      ></div>
    </article>
    <div className="flex gap-4">
      <span className="font-semibold">
        Tags: 
      </span>
      <div className="flex gap-4">
      {tags.map((tag) => (
        <Chip
          key={tag.id}
          startContent={<span>#</span>}
          color="primary"
          variant="flat"

         
        >
          <NextLink href={`/tags/${tag.name}`}>
          {tag.name}
          </NextLink>
        </Chip>
      ))}
      </div>
     
    </div>
    <Divider className="w-full mt-7" />
        <div className="mt-10">
          Binh luận
        </div>
   </div>
  );
}
