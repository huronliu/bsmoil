<template>
  <v-dialog v-model="showEditCoordinator" persistent max-width="85vw">
      <v-card>
        <v-toolbar color="teal" dark>
          <v-toolbar-title>{{station.name}}  的协调器</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn icon @click="cancel">
            <v-icon>close</v-icon>
          </v-btn>
        </v-toolbar>

        <v-expansion-panel popout>
          <v-expansion-panel-content
            v-for="item in coordinators"
            :key="item.seqid"            
          >
            <template v-slot:header>
              <span><v-icon>{{item.icon}}</v-icon> &nbsp;
                  {{ item.coordinator.address? `${item.coordinator.address} (${item.coordinator.name})` : '未设置' }}
              </span>
            </template>
            <v-card class="mx-4">
              <v-list dense>
                <v-list-tile class="mx-3 mt-2">
                  <v-layout>
                    <v-flex xs12>
                      <v-text-field
                        v-model="item.coordinator.address"
                        type="text"
                        name="co_address"
                        label="地址"
                        class="caption"
                        :rules="[rules.required]"
                      ></v-text-field>
                    </v-flex>
                  </v-layout>
                </v-list-tile>
                <v-list-tile class="mx-3 mt-2">
                  <v-layout>
                    <v-flex xs12>
                      <v-text-field
                        v-model="item.coordinator.name"
                        type="text"
                        name="co_name"
                        label="名称"
                        class="caption"
                      ></v-text-field>
                    </v-flex>
                  </v-layout>                 
                </v-list-tile>
              </v-list>
              <v-card-actions class="mt-3 px-3">
                <v-spacer></v-spacer>
                <v-btn color="primary darken-1" flat @click="saveCoordinator(item)">保存</v-btn>
              </v-card-actions>
            </v-card>            
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-card>
    </v-dialog>
</template>

<script>
import api from '../modules/api.js';

export default {
  name: 'EditCoordinator',
  props: {
    showEditDialog: false,
    station: {}
  },
  data() {
    return {    
      showEditCoordinator: false,  
      coordinators: [
        { icon: 'looks_one', seqid: 1, coordinator: {} },
        { icon: 'looks_two', seqid: 2, coordinator: {} },
        { icon: 'looks_3', seqid:3, coordinator: {} },
        { icon: 'looks_4', seqid:4, coordinator: {} },
      ],
      rules: {
        required: value => !!value || "必填"
      }
    }
  },
  watch: {
    showEditDialog(val) {
      this.showEditCoordinator = val;
    }
  },
  methods: {
    cancel() {
      this.showEditCoordinator = false;
      this.$emit('cancel');
    },
    retrieveCoordinators() {
      for (let i = 0; i < 4; i++) {
          this.coordinators[i].coordinator = {};
        }

      if (this.station) {        
        this.$utils.showLoading();
        api.loadCoordinators(this.station)
        .then(result => {
          this.$utils.hideLoading();
          if (result && result.length > 0) {
            result.forEach(co => {
              if (co.seqId >= 1 && co.seqId <= 4) {
                this.coordinators[co.seqId-1].coordinator = co;
              }
            });
          }
        }).catch(err => {
          this.$utils.hideLoading();
          this.$utils.error(`${err.response.data}`);
        });
      }
    }, 
    saveCoordinator(coor) {
      if (coor) {
        this.$util.showLoading();
        api.saveCoordinator(this.station.id, coor.seqid, coor.coordinator)
        .then(() => {
          this.$util.hideLoading();
          this.$util.info('添加协调器信息成功');
          this.retrieveCoordinators();
        }).catch(err => {
          this.$util.hideLoading();
          this.$util.error(`${err.response.data}`);
        });
      }
    }
  },
  activated() {
    this.retrieveCoordinators();
  }
}
</script>

