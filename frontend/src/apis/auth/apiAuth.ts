import axios from 'axios';

const apiAuth = axios.create({
    baseURL: import.meta.env.VITE_AUTH_BASE_URL,
    headers: { "Content-Type": "application/json "},
});

export default apiAuth

