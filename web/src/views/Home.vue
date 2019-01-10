<template>
  <v-container class="pa-5">
    欢迎使用铁塔检测系统
  </v-container>
</template>

<script>
import storage from '../modules/storage.js';

export default {
  name: "Home",
  mounted() {
    if (storage.getAppStarted()) {
      this.$router.replace("/stations");  
    } else {
      this.$router.replace("/start");
    }    
  }
}
</script>
