<template>
  <v-container fluid>
    <mobile-header title="Temp">
      <template slot="left">
        <v-toolbar-side-icon></v-toolbar-side-icon>
      </template>
    </mobile-header>
    <v-slide-y-transition mode="out-in">
      <v-layout column align-center>
        
        <blockquote>
          &#8220;This page is in construction ...&#8221;
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
  name: "Temp",
  data() {
    return {
      name: "L.Y"
    };
  },
  methods: {
  }
};
</script>
