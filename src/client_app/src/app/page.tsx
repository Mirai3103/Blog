
import ArticleCard from '@/components/ArticleCard';
import React from 'react';
import { createClient } from '@/core/client';
import ArticleList from './ArticleList';
interface PageProps {
  searchParams: {
    [key: string]: string | string[] | undefined;
  };
}

export default async function HomePage({ searchParams }: PageProps) {
  const page = parseInt(searchParams.page as string) || 1;
  const pageSize = parseInt(searchParams.pageSize as string) || 12;
  const articles = await createClient({
  }).getAllArticles(page,pageSize,true,null,'CreatedAt')
  return (
 
        <ArticleList initialData={articles.toJSON()} />
  )
}


