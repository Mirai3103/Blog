import React from "react";
import Link from "next/link";
import {
  Card,
  CardHeader,
  CardBody,
  Image,
  Chip,
  Tooltip,
} from "@nextui-org/react";
import NextImage from "next/image";
import { ArticleBriefDto } from "@/core/api_client";


interface ArticleCardProps {
  article: ArticleBriefDto;
}

export default function ArticleCard({ article }: ArticleCardProps) {
  return ( 
    <Card className="py-4 pt-0"  as={Link} href={`/articles/${article.slug}`}>
      <CardHeader className="">
        <Image
        as={NextImage}
        isZoomed
            width={600}
            height={500}
            
          alt="Card background"
          className="object-cover !porse rounded-xl aspect-[6/4]"
          src={article.displayImage}
        />
      </CardHeader>
      <CardBody className="overflow-visible py-2">
        <Chip variant="flat" color="primary">
          {article?.tags?.[0]?.name||"No tag"}
        </Chip>
        <Tooltip content={article.title}>
          <h3 className="text-lg font-bold">
            {article.title!.length > 50
              ? article.title!.slice(0, 50) + "..."
              : article.title}
          </h3>
        </Tooltip>
        <p className="text-sm text-default-500">
          {article.shortDescription!.length > 100
            ? article.shortDescription!.slice(0, 100) + "..."
            : article.shortDescription!}
        </p>
      </CardBody>
    </Card>
  );
}
