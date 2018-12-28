<template>
  <v-container class="pa-5">
    欢迎使用铁塔检测系统
  </v-container>
</template>

<script>
export default {
  name: "Home",
  mounted() {
    this.$router.replace("/stations");
  }
}
</script>
