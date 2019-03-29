<template>
  <v-container class="pa-5">
    欢迎使用铁塔检测系统, 请稍等进入基站管理页面...
  </v-container>
</template>

<script>
import storage from '../modules/storage.js';

export default {
  name: "Home",
  activated() {
    setTimeout(() => {
      if (storage.getAppStarted()) {
        this.$router.replace("/stations");  
      } else {
        this.$router.replace("/start");
      }
    }, 10);        
  }
}
</script>
