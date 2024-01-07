"use client";

import React from "react";
import {
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
  Link,
  NavbarMenuToggle,
  NavbarMenu,
  NavbarMenuItem,
  Input,
} from "@nextui-org/react";
import { MagnifyingGlassIcon } from "@heroicons/react/24/solid";
import DarkModeToggleButton from "@/components/DarkModeToggleButton";
import AuthButton from "./AuthButton";
import { usePathname } from "next/navigation";
import NextLink from "next/link";
const menuItems = [
  {
    display: "Portfolio",
    path: "/",
  },
  {
    display: "Blogs",
    path: "/articles",
  },
  {
    display: "Liên hệ",
    path: "/contact",
  },
];
export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = React.useState(false);
  const pathname = usePathname();


  return (
    <Navbar
      maxWidth="2xl"
      className="mb-unit-6"
      isBordered
      shouldHideOnScroll
      onMenuOpenChange={setIsMenuOpen}
    >
      <NavbarContent>
        <NavbarMenuToggle
          aria-label={isMenuOpen ? "Close menu" : "Open menu"}
          className="md:hidden"
        />
        <NavbarBrand href="/" className="grow-0" as={Link}>
          <p className="font-bold text-inherit">LAFFY</p>
        </NavbarBrand>
        <NavbarItem className="ml-4">
          <NavbarContent className="hidden md:flex gap-4">
            {menuItems.map((item, index) => (
              <NavbarItem key={`${item}-${index}`} isActive={pathname === item.path}>
                <Link
                href={item.path}
                color="foreground"
                as={NextLink}
                {...(pathname === item.path) && { "aria-current": "page" }}
                >
                  {item.display}
                </Link>
              </NavbarItem>
            ))}
           
          </NavbarContent>
        </NavbarItem>
      </NavbarContent>

      <NavbarContent justify="center" className="w-1/3">
        <Input
          classNames={{
            base: "max-w-full h-10",
            mainWrapper: "h-full",
            input: "text-small",
            inputWrapper:
              "h-full font-normal text-default-500 bg-default-400/20 dark:bg-default-500/20",
          }}
          placeholder="Type to search..."
          size="sm"
          startContent={<MagnifyingGlassIcon className="w-5 h-5" />}
          type="search"
        />
      </NavbarContent>
      <NavbarContent justify="end">
        <NavbarItem className="flex">
          <DarkModeToggleButton />
        </NavbarItem>
        <AuthButton />
      </NavbarContent>
      <NavbarMenu>
        {menuItems.map((item, index) => (
          <NavbarMenuItem key={`${item}-${index}`} isActive={pathname === item.path}>
            <Link
              className="w-full"
              as={NextLink}
              href={item.path}
              size="lg"
            >
              {item.display}
            </Link>
          </NavbarMenuItem>
        ))}
      </NavbarMenu>
    </Navbar>
  );
}
