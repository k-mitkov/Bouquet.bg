import axios from 'axios';

export const axiosClient = axios.create({
    headers: {
        "Content-type": "application/json"
    }
});

export const axiosAuthClient = axios.create({
    headers: {
        "Content-type": "application/json"
    }
});
