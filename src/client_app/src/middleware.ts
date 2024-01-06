import { getSession } from '@auth0/nextjs-auth0/edge'
import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'
 
export async function middleware(request: NextRequest) {
  const user =await getSession();
  const accessToken = user?.accessToken || undefined;
  const res= NextResponse.next()
  res.headers.set('Authorization', `Bearer ${accessToken}`)
  res.headers.set('From', 'Next.js')
  res.cookies.set('accessToken', accessToken!)
  return res
}
 