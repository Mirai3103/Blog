import { getAccessToken } from "@auth0/nextjs-auth0/edge";
import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export async function middleware(request: NextRequest) {
  const res = NextResponse.next();

  try {
    const { accessToken } = await getAccessToken();
    res.headers.set("Authorization", `Bearer ${accessToken}`);
    res.cookies.set("accessToken", accessToken!);
  } catch (e) {
    console.log(e);
  }

  res.headers.set("From", "middleware");
  return res;
}
