import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import axios from "axios";

const app = createApp(App)
let baseURL = "";
switch (import.meta.env.MODE) {
  case 'development':
      baseURL = "/dashboard/api";
      break
  default:
      baseURL = window.serverUrl;
      break
}

axios.defaults.baseURL = baseURL;
axios.defaults.withCredentials = true
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.interceptors.request.use(
  config => {
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

app.mount('#app')

