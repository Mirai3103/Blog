"use client";
import {
  Input,
  Select,
  SelectItem,
  Textarea,
  Chip,
  Button,
} from "@nextui-org/react";
import { useForm, SubmitHandler } from "react-hook-form";
import { CreateArticleCommand, TagDto } from "@/core/api_client";
import React from "react";
import slugify from "slugify";
import { useIsClient } from "usehooks-ts";
import RichTextEditor from "@/components/RichTextEditor";
import createArticleValidate from "./createArticleValidate";
import markdownit from "markdown-it";
import { createClient } from "@/core/client";
import { ApiException } from "@/core/api_client";
import axios from "axios";
import { parseValidateError } from "@/utils";
type InputData = CreateArticleCommand & { file: FileList | null };
export default function CreateNewArticle() {
  const isClient = useIsClient();
  const {
    watch,
    register,
    setValue,
    setError,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<InputData>({
    defaultValues: {
      title: "",
      shortDescription: "",
      displayImage: "",
      categoryId: 0,
      content: "",
      slug: "",
      tagIds: [],
      file: null,
    },
  });
  const [tags, setTags] = React.useState<TagDto[]>([]);
  React.useEffect(() => {
    const fetchTags = async () => {
      console.log(createClient({}));
      const tags = await createClient({}).getAllTags(
        1,
        100,
        true,
        null,
        "Name"
      );
      setTags(tags.items!);
    };
    fetchTags();
  }, []);
  console.log({ isSubmitting });

  const onRemoveTag = (tagId: number) => {
    setValue(
      "tagIds",
      [...(watch("tagIds") || [])].filter((id) => id != tagId)
    );
  };
  const onSubmit: SubmitHandler<InputData> = async (data) => {
    console.log(data);
    const client = createClient({});
    const form = new FormData();
    form.append("file", data.file![0]);
    console.log({
      file: data.file,
      form,
    })
    const res = await axios.post("/asp/api/files/", form);
    try{
      const result = await client.createArticle({
        ...data,
        displayImage: res.data as string,
        file: undefined,
        tagIds:[...data.tagIds!] 
      }as any);
    }
    catch(err){
      if(err instanceof ApiException){
        const response = err.response as any;
        const errors = parseValidateError(response.errors);
        for(const key in errors){
          setError(key as any,{
            type:"value",
            message:errors[key]
          })
        }
      }
    }
  };
  const title = watch("title");
  const handlePasteFormMarkdownClick = async () => {
    const text = await navigator.clipboard.readText();
    const md = markdownit();
    const result = md.render(text);
    setValue("content", result);
  };
  return (
    <div className=" flex justify-start flex-col items-center gap-4">
      <h1 className="text-4xl font-bold w-full">Tạo bài viết mới</h1>
      <form
        className="flex w-full flex-col gap-unit-4 px-20"
        onSubmit={handleSubmit(onSubmit)}
      >
        <Input
          label="Tiêu đề bài viết"
          variant="bordered"
          labelPlacement="outside"
          placeholder="Nhập tiêu đề bài viết"
          isInvalid={!!errors.title}
          errorMessage={errors.title?.message}
          size="lg"
          isRequired
          {...register("title", createArticleValidate.title)}
        />
        <div className="flex w-full gap-4 items-end">
          <Input
            label="Đường dẫn tùy chỉnh"
            labelPlacement="outside"
            size="lg"
            variant="bordered"
            className="grow"
            isRequired
            isInvalid={!!errors.slug}
            errorMessage={errors.slug?.message}
            startContent={
              <div className="pointer-events-none flex items-center">
                <span className="text-default-400 text-small">
                  {isClient ? window.location.origin : ""}/articles/
                </span>
              </div>
            }
          value={watch("slug")}
          onChange={(e) => {
            setValue("slug", e.target.value);
          } }
          />
          <Button onClick={() => setValue("slug", slugify(title, {
            strict: true,
            lower: true
          }),{
            shouldValidate:true
          })}>
            {"Tạo tự động"}
          </Button>
        </div>
        <Textarea
          isRequired
          label="Description"
          labelPlacement="outside"
          size="lg"
          placeholder="Nhập mô tả ngắn"
          isInvalid={!!errors.shortDescription}
          errorMessage={errors.shortDescription?.message}
          variant="bordered"
          className="w-full"
          {...register(
            "shortDescription",
            createArticleValidate.shortDescription
          )}
        />

        <div>
          <label className="" htmlFor="file-input">
            Chọn ảnh hiển thị
          </label>
          <div className="mt-2">
            <label htmlFor="file-input" className="sr-only">
              Chọn file
            </label>
            <input
              type="file"
              {...register("file", { required: true })}
              required
              accept="image/*"
              className="block w-full border border-gray-200 shadow-sm rounded-lg text-sm focus:z-10 focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:outline-none dark:focus:ring-1 dark:focus:ring-gray-600
 file:border-0
    file:bg-gray-100 file:me-4
    file:py-3 file:px-4
    dark:file:bg-gray-700 dark:file:text-gray-400"
            />
          </div>
        </div>

        <Select
          selectedKeys={watch("tagIds")}
          label="Thẻ bài viết"
          variant="bordered"
          isMultiline={true}
          selectionMode="multiple"
          placeholder="Chọn thẻ"
          labelPlacement="outside"
          classNames={{
            trigger: "min-h-unit-12 py-2",
          }}
          renderValue={(selectedTags) => {
            return (
              <div className="flex flex-wrap gap-2">
                {selectedTags.map((selectedTag) => (
                  <Chip
                    key={selectedTag.key}
                    onClose={() => onRemoveTag(Number(selectedTag.key))}
                  >
                    {selectedTag.textValue}
                  </Chip>
                ))}
              </div>
            );
          }}
          onSelectionChange={(selectedTags: any) => {
            setValue("tagIds", selectedTags);
          }}
        >
          {tags.map((tag) => (
            <SelectItem key={tag.id!} value={tag.id!}>
              {tag.name}
            </SelectItem>
          ))}
        </Select>
        <div className="flex flex-col gap-unit-2">
          <div className="flex gap-unit-4 items-center">
            <label className="">Nội dung bài viết</label>
            <Button
              color="primary"
              size="sm"
              startContent={
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  strokeWidth={1.5}
                  stroke="currentColor"
                  className="w-6 h-6"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M7.5 7.5h-.75A2.25 2.25 0 0 0 4.5 9.75v7.5a2.25 2.25 0 0 0 2.25 2.25h7.5a2.25 2.25 0 0 0 2.25-2.25v-7.5a2.25 2.25 0 0 0-2.25-2.25h-.75m0-3-3-3m0 0-3 3m3-3v11.25m6-2.25h.75a2.25 2.25 0 0 1 2.25 2.25v7.5a2.25 2.25 0 0 1-2.25 2.25h-7.5a2.25 2.25 0 0 1-2.25-2.25v-.75"
                  />
                </svg>
              }
              onClick={handlePasteFormMarkdownClick}
            >
              Patse Form Markdown
            </Button>
          </div>
          <RichTextEditor
            value={watch("content")}
            onChange={(value) => setValue("content", value)}
          />
        </div>
        <div className="flex justify-end">
          <Button color="primary" size="lg" type="submit" isLoading={isSubmitting}>
            Tạo bài viết
          </Button>
        </div>
      </form>
      <div className="min-h-unit-7xl"></div>
    </div>
  );
}
