import React from 'react'
import { useRouter, useSearchParams,usePathname } from "next/navigation";
import qs from 'qs'
interface QueryState {
    [key: string]: string|undefined 
}
interface Props{
    shallow?:boolean
}

export default function useQueryState({shallow}:Props) {
    const [queryParam, setQueryParam] = React.useState<QueryState>({})
    const router = useRouter()
    const searchParams = useSearchParams()
    const pathname = usePathname()
    // for initial render
    React.useEffect(() => {
        const initialQuery: QueryState = {}         
        searchParams.forEach((value, key) => {
            initialQuery[key] = value
        })
        setQueryParam(initialQuery)
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])
    React.useEffect(() => {
        const queryStr = qs.stringify(queryParam)
        if(shallow){
            router.replace(`${pathname}?${queryStr}`)
        }else{
            router.push(`${pathname}?${queryStr}`)
        }
    }, [queryParam,router,pathname,shallow])
    return [queryParam, setQueryParam] as const
}
