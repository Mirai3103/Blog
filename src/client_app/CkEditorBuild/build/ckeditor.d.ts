/**
 * @license Copyright (c) 2014-2024, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
 */
import { ClassicEditor } from '@ckeditor/ckeditor5-editor-classic';
import { Alignment } from '@ckeditor/ckeditor5-alignment';
import { Autoformat } from '@ckeditor/ckeditor5-autoformat';
import { Bold, Code, Italic, Strikethrough, Subscript, Underline } from '@ckeditor/ckeditor5-basic-styles';
import { BlockQuote } from '@ckeditor/ckeditor5-block-quote';
import { CloudServices } from '@ckeditor/ckeditor5-cloud-services';
import { CodeBlock } from '@ckeditor/ckeditor5-code-block';
import type { EditorConfig } from '@ckeditor/ckeditor5-core';
import { Essentials } from '@ckeditor/ckeditor5-essentials';
import { FindAndReplace } from '@ckeditor/ckeditor5-find-and-replace';
import { FontBackgroundColor, FontColor, FontFamily, FontSize } from '@ckeditor/ckeditor5-font';
import { Heading } from '@ckeditor/ckeditor5-heading';
import { Highlight } from '@ckeditor/ckeditor5-highlight';
import { HtmlEmbed } from '@ckeditor/ckeditor5-html-embed';
import { GeneralHtmlSupport } from '@ckeditor/ckeditor5-html-support';
import { AutoImage, Image, ImageCaption, ImageInsert, ImageResize, ImageStyle, ImageToolbar, ImageUpload } from '@ckeditor/ckeditor5-image';
import { Indent, IndentBlock } from '@ckeditor/ckeditor5-indent';
import { AutoLink, Link, LinkImage } from '@ckeditor/ckeditor5-link';
import { List, ListProperties } from '@ckeditor/ckeditor5-list';
import { Markdown } from '@ckeditor/ckeditor5-markdown-gfm';
import { MediaEmbed, MediaEmbedToolbar } from '@ckeditor/ckeditor5-media-embed';
import { Paragraph } from '@ckeditor/ckeditor5-paragraph';
import { PasteFromOffice } from '@ckeditor/ckeditor5-paste-from-office';
import { RemoveFormat } from '@ckeditor/ckeditor5-remove-format';
import { StandardEditingMode } from '@ckeditor/ckeditor5-restricted-editing';
import { SelectAll } from '@ckeditor/ckeditor5-select-all';
import { SourceEditing } from '@ckeditor/ckeditor5-source-editing';
import { Style } from '@ckeditor/ckeditor5-style';
import { Table, TableCellProperties, TableProperties, TableToolbar } from '@ckeditor/ckeditor5-table';
import { TextTransformation } from '@ckeditor/ckeditor5-typing';
import { Undo } from '@ckeditor/ckeditor5-undo';
declare class Editor extends ClassicEditor {
    static builtinPlugins: (typeof Alignment | typeof AutoImage | typeof AutoLink | typeof Autoformat | typeof BlockQuote | typeof Bold | typeof CloudServices | typeof Code | typeof CodeBlock | typeof Essentials | typeof FindAndReplace | typeof FontBackgroundColor | typeof FontColor | typeof FontFamily | typeof FontSize | typeof GeneralHtmlSupport | typeof Heading | typeof Highlight | typeof HtmlEmbed | typeof Image | typeof ImageCaption | typeof ImageInsert | typeof ImageResize | typeof ImageStyle | typeof ImageToolbar | typeof ImageUpload | typeof Indent | typeof IndentBlock | typeof Italic | typeof Link | typeof LinkImage | typeof List | typeof ListProperties | typeof Markdown | typeof MediaEmbed | typeof MediaEmbedToolbar | typeof Paragraph | typeof PasteFromOffice | typeof RemoveFormat | typeof SelectAll | typeof SourceEditing | typeof StandardEditingMode | typeof Strikethrough | typeof Style | typeof Subscript | typeof Table | typeof TableCellProperties | typeof TableProperties | typeof TableToolbar | typeof TextTransformation | typeof Underline | typeof Undo)[];
    static defaultConfig: EditorConfig;
}
export default Editor;
