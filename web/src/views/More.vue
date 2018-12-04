<template>
  <v-container fluid>
    <mobile-header title="Player on Floor">
      <template>
        
      </template>
    </mobile-header>
    <v-slide-y-transition mode="out-in">
      <v-layout column align-center>        
        <v-btn v-on:click="logout">Logout</v-btn>
      </v-layout>
    </v-slide-y-transition>
  </v-container>
</template>

<script>
export default {
  name: "More",
  methods: {
    logout() {
      // todo: clear session, cache etc
      // don't use vue router to skip the keep-alive cache
      window.location.replace("/login");
    }
  }
};
</script>
