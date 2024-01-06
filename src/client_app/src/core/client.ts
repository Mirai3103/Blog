import axios from 'axios';
import { Client } from './api_client';
interface ClientOptions {
    getAccessToken?: () => Promise<string>;
    baseURL?: string;
}


export function createClient({
    getAccessToken,
    baseURL=process.env.API_URL ||"/asp"

}: ClientOptions): Client {
    const instance = axios.create({
    });
    instance.interceptors.request.use(async (config) => {
        const token = await getAccessToken?.();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const client: Client = new Client(baseURL, instance);
    return client;
}
