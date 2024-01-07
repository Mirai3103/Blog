"use client"

export function getCookie(key:string){
    const cookies = window.document.cookie.split(';')
    const cookie = cookies.find(cookie => cookie.includes(key))
    return cookie?.split('=')[1]
}