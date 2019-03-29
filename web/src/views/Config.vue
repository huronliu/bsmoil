<template>
  <v-container fluid>
    <mobile-header title="配置">
      <template>
      </template>
    </mobile-header>

    <v-card class="my-2">
      <v-list two-line>
        <v-list-tile @click="setting">
          <v-list-tile-avatar>
            <v-icon small>settings</v-icon>
          </v-list-tile-avatar>          
          <v-list-tile-content>
            <v-list-tile-title>服务器配置</v-list-tile-title>
          </v-list-tile-content>
          <v-list-tile-action>
            <v-btn icon ripple>
              <v-icon small>navigate_next</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
    </v-card>
  </v-container>
</template>

<script>
export default {
  name: 'Config',
  methods: {
    setting() {
      this.$router.push('/setting');
    }
  }
}
</script>

