"use client";
import React from "react";
import { useUser } from "@auth0/nextjs-auth0/client";
import { useRouter } from "next/navigation";
import {
  Button,
  NavbarItem,
  Link,
  User,
  Avatar,
  Divider,
  DropdownSection,
} from "@nextui-org/react";
import NextLink from "next/link";
import {
  Dropdown,
  DropdownTrigger,
  DropdownMenu,
  DropdownItem,
} from "@nextui-org/react";

export default function AuthButton() {
  const { user, error, isLoading } = useUser();
  const router = useRouter();
  if (isLoading || error)
    return (
      <>
        <NavbarItem className=" hidden md:flex">
          <Link href="/api/auth/login">Login</Link>
        </NavbarItem>
        <NavbarItem className="hidden md:flex">
          <Button as={Link} color="primary" href="#" variant="flat">
            Sign Up
          </Button>
        </NavbarItem>
      </>
    );

  const { name, picture, email } = user!;
  return (
    <Dropdown placement="bottom-end" size="lg">
      <DropdownTrigger>
        <Avatar
          src={picture!}
          size="md"
          isBordered
          color="primary"
          alt="Profile Picture"
          className="cursor-pointer"
        />
      </DropdownTrigger>
      <DropdownMenu aria-label="Profile Actions" variant="flat">
      <DropdownSection aria-label="Profile " showDivider>
        <DropdownItem   isReadOnly  key="profile" className="h-14 gap-2">
          <p className="font-semibold">{name}</p>
          <p className="font-semibold">{email}</p>
        </DropdownItem>
        </DropdownSection>
        <DropdownSection aria-label="Actions" showDivider>
        <DropdownItem onClick={() => router.push("/articles/create")}>
          Create new article
        </DropdownItem>
        <DropdownItem onClick={() => router.push("/profile")}>
          Profile
        </DropdownItem>
        <DropdownItem onClick={() => router.push("/dashboard")}>
          Dashboard
        </DropdownItem>
        </DropdownSection>
        <DropdownItem onClick={() => router.push("/api/auth/logout")}>
          Log Out
        </DropdownItem>
      </DropdownMenu>
    </Dropdown>
  );
}

const authMenuItems = [
  {
    displayName: "Profile",
    href: "/profile",
  },
  {
    displayName: "Dashboard",
    href: "/dashboard",
  },
  {
    displayName: "Log Out",
    href: "/api/auth/logout",
  },
];
