// app/layout.tsx
import { Providers } from "./_components/providers";
import "./globals.css";
import Header from "./_components/Header";
import { UserProvider } from "@auth0/nextjs-auth0/client";
import NextTopLoader from "nextjs-toploader";
export default async function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  console.log();
  return (
    <html lang="vi">
      <UserProvider>
        <body>
          <Providers>
            <NextTopLoader />
            <Header />
            <main className="container mx-auto p-unit-4">{children}</main>
          </Providers>
        </body>
      </UserProvider>
    </html>
  );
}
