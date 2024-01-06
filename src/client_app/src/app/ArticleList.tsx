"use client";
import ArticleCard from "@/components/ArticleCard";
import { PaginatedListOfArticleBriefDto } from "@/core/api_client";
import { createClient } from "@/core/client";
import useQueryState from "@/hooks/useQueryState";
import { Pagination, Spinner } from "@nextui-org/react";
import React from "react";
import qs from "qs";

interface Props {
  initialData: PaginatedListOfArticleBriefDto;
}
interface QueryParams {
  page: string;
  pageSize: string;
}
export default function ArticleList(props: Props) {
  const [query, setSetQuery] = useQueryState({ shallow: true });
  const [data, setData] = React.useState<PaginatedListOfArticleBriefDto>(
    props.initialData
  );
  const [isFetching, setIsFetching] = React.useState(false);
  React.useEffect(() => {
    const page = parseInt(query.page || props.initialData.pageNumber!.toString())
    if( page == data.pageNumber) return
    const client = createClient({});
    setIsFetching(true);
    window.scroll({
        behavior: "smooth",
        left: 0,
        top: 0,
      });
    client
      .getAllArticles(
        Number(query.page || 1),
        Number(query.pageSize || 12),
        true,
        null,
        "CreatedAt"
      )

      .then((data) => {
   
        setIsFetching(false);
        return data;
      })
      .then(setData);
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [query]);
  const setPage = (page: number) => {
    setSetQuery({ ...query, page: page.toString() });
  };
  return (
    <div className="flex flex-col gap-y-unit-10 mb-unit-20">
      {!isFetching && (<div className="grid mx-auto xl:grid-cols-4  md:grid-cols-3 gap-4 grid-cols-2">
        {data.items!.map((article) => (
          <ArticleCard article={article} key={article.id} />
        ))}
      </div>)}
      {isFetching && (<div className="flex p-unit-28 justify-center items-center">
        <Spinner size="lg" label="Primary" color="primary" labelColor="primary"/>
      </div> )}
      <div className="flex justify-center">
        <Pagination
          size="lg"
          showControls
          total={props.initialData.totalPages!}
          page={Number(query.page || 1)}
          onChange={setPage}
        />
      </div>
    </div>
  );
}
