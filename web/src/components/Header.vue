<template>
  <v-toolbar height="48" class="main_color" dark app style="background-color:#4A87D3">
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
