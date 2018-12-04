<template>
  <v-container fluid>
    <mobile-header title="Player on Floor">
      <template>
        <v-btn v-on:click="scan">Logout</v-btn>
      </template>
    </mobile-header>
    <v-slide-y-transition mode="out-in">
      <v-layout column align-center>
        <img src="@/assets/igt-logo.png" alt="Vuetify.js" class="mb-5">
        <blockquote>&#8220;First, solve the problem. Then, write the code.&#8221;</blockquote>
      </v-layout>
    </v-slide-y-transition>
  </v-container>
</template>

<script>
export default {
  name: "More",
  methods: {
    scan() {
      // todo: clear session, cache etc
      // don't use vue router to skip the keep-alive cache
      window.location.replace("/login");
    }
  }
};
</script>
