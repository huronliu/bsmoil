<template>
  <v-toolbar height="48" class="grey lighten-3" app>
    <slot name="left"></slot>
    <v-toolbar-title>{{title}}</v-toolbar-title>
    <v-spacer></v-spacer>
    <slot></slot>
  </v-toolbar>
</template>

<script>
export default {
  props: {
    title: {
      type: String,
      required: true,
      default: "Title"
    }
  },
  data() {
    return {};
  }
};
</script>
