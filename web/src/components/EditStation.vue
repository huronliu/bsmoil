<template>
  <v-dialog v-model="showEditDialog" scrollable persistent max-width="85vw" style="z-index: 999">
    <v-card class="elevation-20 ">
      <v-card-title
          primary-title
        >
          {{title}}
        </v-card-title>
      <v-card-text style="">
        <v-list two-line subheader dense>
          <v-list-tile class="filter_toolbar_item">
            <v-list-tile-action>
              <v-text-field v-model="station.id"
                label="基站编号"
              ></v-text-field>
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
        <v-list two-line subheader dense>
          <v-list-tile class="filter_toolbar_item">
            <v-list-tile-action>
              <v-text-field v-model="station.title"
                label="基站描述"
              ></v-text-field>
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
        <v-list two-line subheader dense>
          <v-list-tile class="filter_toolbar_item">
            <v-list-tile-action>
              <v-text-field v-model="station.level"
                label="基站等级"
              ></v-text-field>
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
      </v-card-text>
      <v-divider></v-divider>   
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn flat @click="cancel">取消</v-btn>
        <v-btn color="primary" flat @click="save">确定</v-btn>
      </v-card-actions>
    </v-card>
    <v-spacer></v-spacer>
  </v-dialog>
</template>

<script>
export default {
  name: 'EditStation',
  props: {
    showEditDialog: false,
    isNew: true,
    station: {}
  },
  data() {
    return {
    }
  },
  computed: {
    title: {
      get() {
        return this.isNew? "新增基站" : "编辑基站";        
      }
    }
  },
  methods: { 
    save() {
      this.$emit('save', this.station);
    },
    cancel() {
      this.$emit('cancel');
    }
  },
  activated: function() {
   
  },
  beforeMount: function() {
    
  }
}
</script>

<style>
.filter_toolbar_item {
  
}
.filter_toolbar_item >>> .v-list__tile {
  height: 40px !important;
}
.v-text-field--single-line >>> .v-input__slot {
  height: 36px;
  min-height: 36px;
}
.v-text-field--single-line >>> .v-input__control {
  min-height: 36px;
}
.v-text-field--single-line >>> input {
  margin-top: 0;
}
.filter_toolbar_item >>> .v-select__selections {
  flex-wrap: nowrap;
}
</style>
