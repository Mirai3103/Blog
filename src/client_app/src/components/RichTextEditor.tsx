"use client";
import Editor from "ckeditor5-custom-build/build/ckeditor";

import { CKEditor } from "@ckeditor/ckeditor5-react";
import React from "react";

interface RichTextEditorProps {
  value?: string;
  onChange?: (value: string) => void;
}
 function RichTextEditorUnMemo(props: RichTextEditorProps) {
  return (
    <div className="prose
    cursor-text
    bg-white
    w-full !max-w-screen-2xl relative  inline-flex  flex-row items-center shadow-sm px-3 gap-3 border-medium border-default-200    min-h-unit-12 rounded-large !h-auto transition-background !duration-150 transition-colors motion-reduce:transition-none py-2">
      <CKEditor
        editor={Editor}
        id={"editor"}
        data={props?.value||""}
        onChange={(event, editor) => {
          const data = editor.getData();
          console.log({ event, editor, data });
          props?.onChange?.(data);
        }}
      />
    </div>
  );
}

 const RichTextEditor = React.memo(RichTextEditorUnMemo);

export default RichTextEditor;