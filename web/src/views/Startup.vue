<template>
  <v-container class="pa-0">
    
    <v-carousel hide-delimiters height="100vh" max="100vw">      
      <v-carousel-item
        v-for="(item,i) in items"
        :key="i"
      >
        <v-img :src="item.src" width="100vw" height="100vh" style="width: 100vw; height: 100vh"></v-img>
      </v-carousel-item>
    </v-carousel>

    <v-btn absolute outline color="teal" small z-index="1111" style="top: 50px; left: 50%; margin-left: -43px" @click="onStartClick">开始使用</v-btn>
  </v-container>
</template>

<script>
import storage from '../modules/storage.js';

export default {
  name: "Startup",
  data() {
    return {
      baseUrl: null,
      items: []
    }
  },
  computed: {
  },
  mounted() {
    this.baseUrl = process.env.BASE_URL;
    if (process.env.BASE_URL) {
      this.baseUrl = process.env.BASE_URL;
    } else {
      if (window.$isAndroid) {
        this.baseUrl = '/android_asset/www/';
      } else {
        this.baseUrl = "/";
      }
    }
    
    this.items = [
        { src: this.baseUrl + 'images/start1.jpg' },
        { src: this.baseUrl + 'images/start2.jpg' },
        { src: this.baseUrl + 'images/start3.jpg' }
    ];
    console.log(JSON.stringify(this.items));
  },
  methods: {
    onStartClick() {
      storage.setAppStarted();
      this.$router.replace("/stations");
    }
  }
}
</script>

<style scoped>
</style>
