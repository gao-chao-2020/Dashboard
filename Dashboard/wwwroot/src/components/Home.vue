<template>
  <div>
    <h1>Home</h1>
    {{ message }}
  </div>
</template>
<script setup>
import axios from "axios";
import { ref, onMounted, onBeforeUnmount } from "vue";
const message = ref("hello");
let timer;
function health() {
  if (window.pollingInterval == "%(pollingInterval)") {
    window.pollingInterval = 1000;
  }
  axios
    .get("/health")
    .then((response) => {
      message.value = response.data;
      timer = setTimeout(() => {
        health();
      }, pollingInterval);
    })
    .catch((error) => {
      console.log(error);
    });
}

onMounted(() => {
  health();
});

onBeforeUnmount(() => {
  clearTimeout(timer);
});
</script>
