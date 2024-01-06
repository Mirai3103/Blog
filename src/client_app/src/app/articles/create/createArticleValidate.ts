import { CreateArticleCommand } from "@/core/api_client";
import { RegisterOptions } from "react-hook-form";

const titleValidate: RegisterOptions<CreateArticleCommand, "title"> = {
  required: "Tiêu đề không được để trống",
  minLength: {
    value: 10,
    message: "Tiêu đề phải có ít nhất 10 ký tự",
  },
  maxLength: {
    value: 100,
    message: "Tiêu đề phải có nhiều nhất 100 ký tự",
  },
};
const shortDescriptionValidate: RegisterOptions<CreateArticleCommand, "shortDescription"> = {
  required: "Mô tả ngắn không được để trống",
  minLength: {
    value: 10,
    message: "Mô tả ngắn phải có ít nhất 10 ký tự",
  },
  maxLength: {
    value: 100,
    message: "Mô tả ngắn phải có nhiều nhất 100 ký tự",
  },
};
const displayImageValidate: RegisterOptions<CreateArticleCommand, "displayImage"> = {
  required: "Ảnh đại diện không được để trống",
};
const categoryIdValidate: RegisterOptions<CreateArticleCommand, "categoryId"> = {
  required: "Danh mục không được để trống",
};
const contentValidate: RegisterOptions<CreateArticleCommand, "content"> = {
  required: "Nội dung không được để trống",
};
const slugValidate: RegisterOptions<CreateArticleCommand, "slug"> = {
  required: "Đường dẫn không được để trống",
  pattern: {
    value: /^[a-z0-9]+(?:-[a-z0-9]+)*$/,
    message: "Đường dẫn không hợp lệ",
  },
  minLength: {
    value: 10,
    message: "Đường dẫn phải có ít nhất 10 ký tự",
  },
  maxLength: {
    value: 100,
    message: "Đường dẫn phải có nhiều nhất 100 ký tự",
  },
};


const createArticleValidate = {
  title: titleValidate,
  shortDescription: shortDescriptionValidate,
  displayImage: displayImageValidate,
  categoryId: categoryIdValidate,
  content: contentValidate,
  slug: slugValidate,
};

export default createArticleValidate;