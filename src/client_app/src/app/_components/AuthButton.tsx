"use client";
import React from "react";
import { useUser } from "@auth0/nextjs-auth0/client";
import { useRouter } from "next/navigation";
import {
  Button,
  NavbarItem,
  Link,
  Avatar,
  DropdownSection,
} from "@nextui-org/react";
import {
  Dropdown,
  DropdownTrigger,
  DropdownMenu,
  DropdownItem,
} from "@nextui-org/react";

export default function AuthButton() {
  const { user, error, isLoading } = useUser();
  const router = useRouter();
  if (isLoading || error|| !user)
    return (
      <>
        <NavbarItem className=" hidden md:flex">
          <Button as={Link} color="primary" href="/api/auth/login">Đăng nhập</Button>
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
         Tạo bài viết
        </DropdownItem>
        <DropdownItem onClick={() => router.push("/profile")}>
          Trang cá nhân
        </DropdownItem>
        <DropdownItem onClick={() => router.push("/dashboard")}>
          Quản lý bài viết
        </DropdownItem>
        </DropdownSection>
        <DropdownItem onClick={() => router.push("/api/auth/logout")}>
          Đăng xuất
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
