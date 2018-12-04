<template>
  <v-container fluid>
    <mobile-header title="Player on Table">
      <template slot="left">
        <v-toolbar-side-icon></v-toolbar-side-icon>
      </template>
      <template>
        <v-btn icon v-on:click="scan()">
          <v-icon>crop_free</v-icon>
        </v-btn>
      </template>
    </mobile-header>
    <v-slide-y-transition mode="out-in">
      <v-layout column align-center>
        
        <blockquote>
          &#8220;First, solve the problem. Then, write the code.&#8221;
          <footer>
            <small>
              <em>&mdash;{{name}}</em>
            </small>
          </footer>
        </blockquote>
      </v-layout>
    </v-slide-y-transition>
  </v-container>
</template>

<script>
export default {
  name: "PlayerOnTable",
  data() {
    return {
      name: "John Johnson"
    };
  },
  methods: {
    scan() {
      this.$emit("logout");
    }
  }
};
</script>
