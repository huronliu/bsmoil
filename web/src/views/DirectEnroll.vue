<template>
  <v-container fluid>
    <mobile-header :title="pageTitle">
      <template slot="left">
        <v-btn icon large v-on:click="convert">
          <v-icon>swap_horiz</v-icon>
        </v-btn>
      </template>
      <template>
        <v-btn icon large v-on:click="scan">
          <v-icon>crop_free</v-icon>
        </v-btn>
      </template>
    </mobile-header>
    <v-stepper v-model='currentStep' >
      <v-stepper-header>
        <v-stepper-step class='pt-1 pb-1 stepper_header_icon' :complete='currentStep > 5' key='1-step' step=1 :rules='[() => stepValidate_1]' editable>
        </v-stepper-step>
        <v-divider key='1'></v-divider>

        <v-stepper-step class='pt-1 pb-1 stepper_header_icon' :complete='currentStep > 5' key='2-step' step=2 :rules='[() => stepValidate_2]' editable>
        </v-stepper-step>
        <v-divider key='2'></v-divider>

        <v-stepper-step class='pt-1 pb-1 stepper_header_icon' :complete='currentStep > 5' key='3-step' step=3 :rules='[() => stepValidate_3]' editable>
        </v-stepper-step>
        <v-divider key='3'></v-divider>

        <v-stepper-step class='pt-1 pb-1 stepper_header_icon' :complete='currentStep > 5' key='4-step' step=4 :rules='[() => stepValidate_4]' editable>
        </v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content step='1' v-touch="{left: () => swipeForward(), right: () => swipeBack()}">
          <v-form ref='form1' v-model='stepValidate_1' lazy-validation>
            <transition name="slide-fade">
              <v-text-field ref="PlayerID_TextBox" v-model='playerID' :rules='playerIDRules' v-show="convertTempPlayer" v-on:blur="searchPlayerByID()"  append-icon="search" @click:append="searchPlayerByID()" :placeholder="$t('enroll.playerIDHint')">
                <v-icon large color="indigo" slot="prepend" @click.native="swipeCard()">{{swipCardIcon}}</v-icon>
              </v-text-field>
            </transition>
            <v-layout row wrap>
              <v-flex xs7>
                <v-text-field v-model='firstName' :rules='firstNameRules' :placeholder="firstNameHint" :label="$t('enroll.firstName')" v-on:blur="UpperFirstName()" required maxlength=40></v-text-field>
                <v-text-field v-model='middleName' :rules='middleNameRules' :label="$t('enroll.middleName')" v-on:blur="UpperMiddleName()" maxlength=40></v-text-field>
                <v-text-field v-model='lastName' :rules='lastNameRules' :placeholder="lastNameHint" :label="$t('enroll.lastName')" v-on:blur="UpperLastName()" required maxlength=40></v-text-field>
              </v-flex>

              <v-flex xs5>
                <v-layout align-center justify-center column fill-height>
                  <v-flex>
                    <v-avatar size='110px' tile class='mt-3'>
                      <img :src='photoSrc'>
                    </v-avatar>
                  </v-flex>

                  <v-flex>
                    <v-btn class='pl-1 pr-1' outline color='indigo' v-on:click='TakePhoto()'>
                      <v-icon Outlined>photo_camera</v-icon>
                      <span style='font-size:10px'>{{$t('enroll.takePhoto')}}</span>
                    </v-btn>
                  </v-flex>
                </v-layout>
              </v-flex>
            </v-layout>

            <v-layout row justify-space-between>
              <v-flex xs7>
                <v-select clearable v-model='title' :items='titles' item-text="Title" item-value="Title" :label="$t('enroll.title')"></v-select>
              </v-flex>
              <v-flex xs5 pl-3>
                <v-select v-model='gender' :items='genders' item-text="text" item-value="value" :label="$t('enroll.gender')"></v-select>
              </v-flex>
            </v-layout>

            <v-menu ref='birthdayMenu' :close-on-content-click='false' v-model='birthdayMenu' :nudge-right='40' lazy transition='scale-transition' offset-y full-width min-width='290px'>
              <v-text-field slot='activator' v-model='computedBirthdateFormatted' :rules='birthdateRules' required :label="$t('enroll.birthday')" readonly></v-text-field>
              <v-date-picker ref='birthdayPicker' v-model='birthdateInit' :max='maxBirthDate' min='1900-01-01' @change='saveBirthday' reactive scrollable></v-date-picker>
            </v-menu>
            <v-autocomplete clearable v-model="language" :items="languages" item-text="Description" item-value="Description" :label="$t('enroll.language')"></v-autocomplete>

            <v-layout row justify-space-between>
              <v-flex>
                <v-layout justify-end>
                   <v-btn color='primary' @click='nextStepValidate1()'>
                     {{$t('enroll.next')}}
                   </v-btn>
                   <v-btn color='primary' v-show='showSubmit' @click='submit()'>
                     {{submitButtonName}}
                   </v-btn>
                </v-layout>
              </v-flex>
            </v-layout>
          </v-form>
        </v-stepper-content>

        <v-stepper-content step='2' v-touch="{left: () => swipeForward(), right: () => swipeBack()}">
          <v-text-field  v-model='playerID' v-show="convertTempPlayer" readonly :prepend-icon="swipCardIcon" append-icon="search" :placeholder="$t('enroll.playerIDHint')"></v-text-field>
          <v-form ref='form2' v-model='stepValidate_2' lazy-validation>
            <v-text-field v-model='idNumber' :rules='idRules' :label="`* ${idTypeDisplayText}`" required maxlength=32></v-text-field>
            <v-layout row wrap>
              <v-flex xs8>
                <v-menu ref='idExpireDateMenu' :close-on-content-click='false' v-model='idExpireDateMenu' :nudge-right='40' lazy transition='scale-transition' offset-y full-width min-width='290px'>
                  <v-text-field slot='activator' v-model='computedIDExpireDateFormatted' :rules='idExpireDateRules' :label="$t('enroll.expirationDate')" readonly></v-text-field>
                  <v-date-picker ref='idExpireDatePicker' v-model='idExpireDate' :min='today' @change='saveIDExpireDate'></v-date-picker>
                </v-menu>
              </v-flex>

              <v-flex xs4>
                <v-checkbox :label="$t('enroll.verified')" v-model='idVerified' class='idExpVerify'></v-checkbox>
              </v-flex>
            </v-layout>
            <v-autocomplete clearable v-model="idCountry" :items="countries" item-text="name" item-value="countryId" :label="$t('enroll.country')"></v-autocomplete>
            <v-autocomplete clearable v-model='idState' :items='statesForId' item-text="name" item-value="abbrev" :label="$t('enroll.state')" :disabled="idStateDisabled"></v-autocomplete>
            <v-layout justify-end>
              <v-btn color='primary' @click='nextStepValidate2()'>
                {{$t('enroll.next')}}
              </v-btn>
              <v-btn color='primary' v-show='showSubmit'   @click='submit()'>
                {{submitButtonName}}
              </v-btn>
            </v-layout>
          </v-form>
        </v-stepper-content>

        <v-stepper-content step='3' v-touch="{left: () => swipeForward(), right: () => swipeBack()}">
          <v-text-field  v-model='playerID' v-show="convertTempPlayer" readonly :prepend-icon="swipCardIcon" append-icon="search" :placeholder="$t('enroll.playerIDHint')"></v-text-field>
          <v-form ref='form3' v-model='stepValidate_3' lazy-validation>
            <v-text-field v-model='homeAddress' :label="addressDisplayText" maxlength=128></v-text-field>
            <v-autocomplete clearable v-model="homeCountry" :items="countries" item-text="name" item-value="countryId" :label="$t('enroll.country')"></v-autocomplete>
            <v-autocomplete clearable v-model='homeState' :items='statesForAddress' item-text="name" item-value="abbrev" :label="$t('enroll.state')" :disabled="homeStateDisabled"></v-autocomplete>
            <v-text-field v-model='homeCity' :label="$t('enroll.city')" v-on:blur="UpperCity()" maxlength=128></v-text-field>
            <v-text-field v-model='homePostalCode' :label="$t('enroll.zipCode')" maxlength=32></v-text-field>
            <v-text-field v-model='homePhone' :mask='phonemask' :placeholder='phonemask' :label="$t('enroll.homePhone')" maxlength=16></v-text-field>

            <v-layout row justify-space-between>
              <v-flex xs5 sm5 md5>
                <v-text-field v-model='mobilePhone' :mask='phonemask' :placeholder='phonemask' :label="$t('enroll.mobile')" maxlength=16></v-text-field>
              </v-flex>
              <v-flex xs7 sm4 md4>
                <v-checkbox v-model='permitSMS' :label="$t('enroll.permitTextMsg')" class='permitsms_checkbox'></v-checkbox>
              </v-flex>
            </v-layout>
            <v-text-field v-model='mainPhone' :label="mainPhoneTypeDesc" v-show="showMainPhoneType" maxlength=16></v-text-field>
            <v-text-field v-model='email' :rules='emailRules' :label="emailDisplayText" required maxlength=64></v-text-field>

            <v-layout justify-end>
              <v-btn color='primary' @click='nextStepValidate3()'>
                {{$t('enroll.next')}}
              </v-btn>
              <v-btn color='primary' v-show='showSubmit' @click='submit()'>
                {{submitButtonName}}
              </v-btn>
            </v-layout>
          </v-form>
        </v-stepper-content>
        <v-stepper-content step='4'>
          <v-text-field v-model='playerID' v-show="convertTempPlayer" readonly :prepend-icon="swipCardIcon" append-icon="search" :placeholder="$t('enroll.playerIDHint')"></v-text-field>
          <v-form ref='form4' v-model='stepValidate_4' lazy-validation>
            <v-layout v-touch="{right: () => swipeBack()}" column>
              <v-select v-model='preferredComMethod' :items='preferredCommunicationMethods' item-text="PreferredTypeDescription" item-value="PreferredTypeID" :label="$t('enroll.prefMethodOfCommunication')"></v-select>
              <v-select v-model='preferredPhone' hide-details :items='preferredPhones' item-text="Description" item-value="Abbrev" :label="$t('enroll.preferredPhone')"></v-select>
              <v-checkbox class='protocol_checkbox' hide-details v-model='agreeProtocol' :rules="agreeProtocolRules">
                <template slot='label'>
                  {{regProtocolMandatory?'* ':''}}{{$t('enroll.accept')}}&nbsp;
                  <a href='#' @click.stop.prevent='dialogProtocol = true'>{{$t('enroll.playerRegProtocol')}}</a>.
                </template>
              </v-checkbox>
            </v-layout>
            <signature-pad ref="signPad" :language-locale="this.$store.state.settings.language.value" :signature-required="signatureMandatory" />
            <v-layout justify-end>
              <v-btn color='primary' @click='submit()'>
                {{submitButtonName}}
              </v-btn>
            </v-layout>
          </v-form>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
    <v-dialog v-model="photoDialog" fullscreen hide-overlay transition="dialog-bottom-transition">
      <v-card>
        <v-toolbar dark color="primary">
          <v-btn icon dark @click.native="photoDialog = false">
            <v-icon>close</v-icon>
          </v-btn>
          <v-toolbar-title>{{$t('enroll.takePhoto')}}</v-toolbar-title>
          <v-spacer></v-spacer>
          <v-toolbar-items>
            <v-btn dark flat @click="CropImage()">{{$t('common.save')}}</v-btn>
          </v-toolbar-items>
        </v-toolbar>
        <v-layout row justify-center>
          <vue-cropper
              ref='cropper'
              :guides="true"
              :view-mode="2"
              drag-mode="crop"
              :auto-crop-area="0.8"
              :minCropBoxWidth="120"
              :minCropBoxHeight="120"
              :background="true"
              :rotatable="false"
              :aspectRatio="1"
              :zoomOnTouch="false"
              :src="cameraOriginalPhotoSrc"
              alt="Source Image"
              :img-style="{ 'width': '300px', 'height': '300px' }">
          </vue-cropper>
        </v-layout>
      </v-card>
    </v-dialog>
    <v-dialog v-model="inScan" fullscreen hide-overlay transition="dialog-bottom-transition">
      <v-card>
        <v-toolbar dark color="primary">
          <v-btn icon dark @click="stopScan()">
            <v-icon>close</v-icon>
          </v-btn>
          <v-toolbar-title>{{$t('enroll.scanDL')}}</v-toolbar-title>
          <v-spacer></v-spacer>
        </v-toolbar>
      </v-card>
    </v-dialog>
    <v-dialog v-model="inSwipe" fullscreen hide-overlay transition="dialog-bottom-transition">
      <v-card>
        <v-toolbar dark color="primary">
          <v-btn icon dark @click="stopSwipe()">
            <v-icon>close</v-icon>
          </v-btn>
          <v-toolbar-title>{{swipePrompt}}</v-toolbar-title>
          <v-spacer></v-spacer>
        </v-toolbar>
        <v-layout column align-center>
          <img src="@/assets/swipe.png" alt="Vuetify.js" class="mb-5">
        </v-layout>
      </v-card>
    </v-dialog>
    <v-dialog v-model='dialogProtocol' scrollable max-width='450'>
      <v-card>
        <v-card-title style="font-weight:bold">{{$t('enroll.playerRegProtocol')}}</v-card-title>
        <v-card-text>
          <pre>{{RegistrationProtocol}}</pre>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color='green darken-1' flat @click.native="dialogProtocol = false; agreeProtocol = false">{{$t('enroll.disagree')}}</v-btn>
          <v-btn color='green darken-1' flat @click.native="dialogProtocol = false; agreeProtocol = true">{{$t('enroll.agree')}}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model='dialogRequireEmail' persistent scrollable max-width='450'>
      <v-card>
        <v-card-title style="font-weight:bold">{{$t('common.warning')}}</v-card-title>
        <v-card-text>
            {{$t('enroll.email_required')}}
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color='green darken-1' flat @click.native="dialogRequireEmail = false; includeEmail = false; submitPlayerInfo(true)">{{$t('common.yes')}}</v-btn>
          <v-btn color='green darken-1' flat @click.native="dialogRequireEmail = false; includeEmail = true; currentStep = 3;">{{$t('common.no')}}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model='dialogFoundDuplicate' persistent scrollable max-width='450'>
      <v-card>
        <v-card-title style="font-weight:bold">{{$t('common.warning')}}</v-card-title>
        <v-card-text>
            {{duplicatePromptMsg}}
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn v-if='enableDuplicateEnroll' color='green darken-1' flat @click.native="dialogFoundDuplicate = false; submitPlayerInfo(false)">{{$t('common.yes')}}</v-btn>
          <v-btn color='green darken-1' flat @click.native="dialogFoundDuplicate = false; currentStep = 1;">{{duplicateConfirmMsg}}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model='dialogIdExpired' persistent max-width='450'>
      <v-card>
        <v-card-title style="font-weight:bold">{{$t('common.error')}}</v-card-title>
        <v-card-text>
            {{$t('enroll.id_expired', { idType: this.idTypeDisplayText })}}
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color='green darken-1' flat @click.native="dialogIdExpired = false;">{{$t('common.close')}}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model='dialogDiscreetPlayerFound' persistent max-width='450'>
      <v-card>
        <v-card-title style="font-weight:bold">{{$t('common.error')}}</v-card-title>
        <v-card-text>
            Player ID: {{ discreetPlayerId }}, {{$t('enroll.discreetPlayer')}}
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color='green darken-1' flat @click.native="dialogDiscreetPlayerFound = false;">{{$t('common.close')}}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import MobileHeader from "@/components/MobileHeader";
import directEnrollCache from "@/modules/directEnrollCache";
import SignaturePad from "@igt/signature-pad/src/SignaturePad.vue";
import VueCropper from "vue-cropperjs";
import cmdScanHandler from "@/modules/cmdScanHandler";
import _ from "lodash";
import moment from "moment";
import { setTimeout } from "timers";

export default {
  name: "DirectEnroll",
  components: {
    MobileHeader,
    SignaturePad,
    VueCropper
  },
  data() {
    return {
      inScan: false,
      inSwipe: false,
      enableDuplicateEnroll: false,
      duplicatePromptMsg: "",
      duplicateConfirmMsg: "",
      swipePrompt: "",
      currentStep: 1,
      steps: 4,

      convertTempPlayer: false,
      enableSwipeCard: false,
      swipCardIcon: null,
      pageTitle: "",
      playerID: "",
      playerID_previous: "",
      firstNameHint: "",
      lastNameHint: "",
      submitButtonName: "",

      photoDialog: false,

      firstName: "",
      middleName: "",
      lastName: "",
      playerIDRules: [
        v => !this.convertTempPlayer || !!v || this.$t("common.required")
      ],
      firstNameRules: [
        v => !!v || this.$t("enroll.firstName_required"),
        v => (v && v.length <= 40) || this.$t("enroll.firstName_maxLength")
      ],
      middleNameRules: [
        v => !v || (v.length <= 40 || this.$t("enroll.middleName_maxLength"))
      ],
      lastNameRules: [
        v => !!v || this.$t("enroll.lastName_required"),
        v => (v && v.length <= 40) || this.$t("enroll.lastName_maxLength")
      ],
      agreeProtocolRules: [
        v => !this.regProtocolMandatory || !!v || this.$t("common.required")
      ],
      idNumber: "",
      idRules: [
        v =>
          !!v ||
          this.$t("enroll.id_required", { idType: this.idTypeDisplayText })
      ],
      idExpireDateRules: [
        v => {
          if (v && moment(this.idExpireDate).isBefore(Date.now(), "day")) {
            return this.$t("enroll.id_expired", {
              idType: this.idTypeDisplayText
            });
          } else {
            return true;
          }
        }
      ],
      email: "",
      emailRules: [
        v => !v || (/.+@.+/.test(v) || this.$t("enroll.email_valid"))
      ],
      gender: null,
      title: null,
      agreeProtocol: false,
      birthdate: null,
      birthdateInit: null,
      birthdayMenu: false,
      maxBirthDate: null,
      birthdateRules: [v => !!v || this.$t("enroll.birthday_required")],
      idExpireDate: null,
      dialogIdExpired: false,
      idExpireDateMenu: false,
      idVerified: false,
      idState: null,
      idCountry: null,
      idStateDisabled: true,
      language: null,
      homeAddress: null,
      homeCountry: null,
      homeCity: null,
      homeState: null,
      homeStateDisabled: true,
      homePostalCode: null,
      homePhone: null,
      mobilePhone: null,
      phonemask: null,
      permitSMS: false,
      preferredPhone: null,
      preferredPhones: null,
      mainPhone: null,
      mainPhoneType: null,
      mainPhoneTypeDesc: null,
      showMainPhoneType: false,
      preferredComMethod: null,
      dialogProtocol: false,
      photoSrc: "/assets/img/photo.png",
      cameraOriginalPhotoSrc: null,
      stepValidate_1: true,
      stepValidate_2: true,
      stepValidate_3: true,
      stepValidate_4: true,
      regProtocolMandatory: true,
      signatureMandatory: true,
      showSubmit: false,
      fullscreenDlg: false,
      RegistrationProtocol: "",
      enrollRequireEmail: true,
      dialogRequireEmail: false,
      includeEmail: true,
      dialogFoundDuplicate: false,
      dialogDiscreetPlayerFound: false,
      discreetPlayerId: "",
      genders: null
    };
  },
  computed: {
    today: {
      get() {
        return moment({
          hour: 0,
          minute: 0,
          seconds: 0,
          milliseconds: 0
        }).format("YYYY-MM-DD");
      }
    },
    titles: {
      get() {
        return directEnrollCache.getTitleList();
      }
    },
    countries: {
      get() {
        return directEnrollCache.getCountryList();
      }
    },
    statesForId: {
      get() {
        return directEnrollCache.getStatesOfCountry(this.idCountry);
      }
    },
    statesForAddress: {
      get() {
        return directEnrollCache.getStatesOfCountry(this.homeCountry);
      }
    },
    languages: {
      get() {
        return directEnrollCache.getLanguageList();
      }
    },
    preferredCommunicationMethods: {
      get() {
        return directEnrollCache.getPreferredCommunicationMethodList();
      }
    },
    computedBirthdateFormatted: {
      get() {
        return this.formatDate(this.birthdate);
      },
      set(newVal) {
        this.birthdate = this.parseDate(newVal);
      }
    },
    computedIDExpireDateFormatted: {
      get() {
        return this.formatDate(this.idExpireDate);
      },
      set(newVal) {
        this.idExpireDate = this.parseDate(newVal);
      }
    },
    identificationType: {
      get() {
        return this.$config.get(
          "setting_default_identification_type",
          "Driver License"
        );
      }
    },
    idTypeDisplayText: {
      get() {
        let config = this.$config.getObj("setting_default_identification_type");
        if (config) {
          return config.displayText || config.value;
        }

        return "Driver License";
      }
    },
    emailDisplayText: {
      get() {
        let config = this.$config.getObj("setting_default_email_type");
        if (config) {
          return config.displayText || config.value;
        }

        return "Main Email";
      }
    },
    emailType: {
      get() {
        let config = this.$config.getObj("setting_default_email_type");
        if (config) {
          return config.value;
        }

        return "Main Email";
      }
    },
    addressDisplayText: {
      get() {
        let config = this.$config.getObj("setting_default_address_type");
        if (config) {
          return config.displayText || config.value;
        }

        return "Home Address";
      }
    },
    addressType: {
      get() {
        let config = this.$config.getObj("setting_default_address_type");
        if (config) {
          return config.value;
        }

        return "Home";
      }
    },
    signatureHasError: {
      get() {
        if (this.signatureMandatory) {
          return this.$refs.signPad.validateSignature();
        } else {
          return false;
        }
      }
    }
  },
  watch: {
    birthdayMenu(val) {
      val &&
        this.$nextTick(() => {
          if (this.birthdate) {
            this.birthdateInit = this.birthdate;
          }
          this.$refs.birthdayPicker.activePicker = "YEAR";
          this.datePickerScrollToSelectedYear(this.$refs.birthdayPicker);
        });
    },
    idExpireDateMenu(val) {
      val &&
        this.$nextTick(() => {
          this.$refs.idExpireDatePicker.activePicker = "YEAR";
          this.datePickerScrollToSelectedYear(this.$refs.idExpireDatePicker);
        });
    },
    idExpireDate(newVal) {
      if (moment(newVal).isBefore(Date.now(), "day")) {
        this.dialogIdExpired = true;
      }
    },
    idCountry(val) {
      this.idState = null;
      const states = directEnrollCache.getStatesOfCountry(val);
      if (states.length > 0) {
        this.idStateDisabled = false;
      } else {
        this.idStateDisabled = true;
      }
    },
    homeCountry(val) {
      this.homeState = null;
      const states = directEnrollCache.getStatesOfCountry(val);
      if (states.length > 0) {
        this.homeStateDisabled = false;
      } else {
        this.homeStateDisabled = true;
      }

      if (val === "840") {
        this.phonemask = this.$config.get(
          "setting_phone_number_mask",
          "(###) ###-####"
        );
      } else {
        this.phonemask = null;
      }
    }
  },
  methods: {
    swipeForward() {
      if (this.currentStep == 4) {
        return;
      }
      if (this.currentStep == 3) {
        this.nextStepValidate3();
      } else {
        this.currentStep = this.currentStep + 1;
      }
    },
    swipeBack() {
      if (this.currentStep == 1) {
        return;
      }
      this.currentStep = this.currentStep - 1;
    },
    onBackKeyDown() {
      if (this.inScan) {
        this.stopScan();
      } else if (this.photoDialog) {
        this.photoDialog = false;
      } else if (this.inSwipe) {
        this.stopSwipe();
      } else if (this.dialogDiscreetPlayerFound) {
        this.dialogDiscreetPlayerFound = false;
      } else {
        window.history.back();
      }
    },
    datePickerScrollToSelectedYear(picker) {
      this.$nextTick(() => {
        // A ticky method to make the DatePicker always scroll to the current selected year when popup.
        setTimeout(() => {
          const yearContainer = picker.$el.querySelector(
            "ul.v-date-picker-years"
          );
          const activeItem = yearContainer.querySelector("li.active");
          if (activeItem) {
            yearContainer.scrollTop =
              activeItem.offsetTop -
              yearContainer.offsetHeight / 2 +
              activeItem.offsetHeight / 2;
          } else {
            yearContainer.scrollTop =
              yearContainer.scrollHeight / 2 - yearContainer.offsetHeight / 2;
          }
        }, 200);
      });
    },
    convert() {
      this.convertTempPlayer = !this.convertTempPlayer;
      if (this.convertTempPlayer) {
        this.currentStep = 1;
        this.submitButtonName = this.$t("common.update");
        this.pageTitle = this.$t("enroll.updatePlayer");
        this.clearData();
        this.$nextTick(() => this.$refs.PlayerID_TextBox.focus());
      } else {
        this.playerID = "";
        this.firstNameHint = "";
        this.lastNameHint = "";
        this.submitButtonName = this.$t("common.submit");
        this.pageTitle = this.$t("enroll.enrollPlayer");
      }
    },
    processSwipeResult: function(result) {
      console.log("result sent!");
      this.stopSwipe();
      this.clearData();
      this.playerID = "";
      if (result && result.length > 0) {
        this.searchPlayerByCardId(result);
      }
    },
    startSwipeCallback: function(result) {
      console.log("connect return!");
      console.log(result);
      if (result === "YES") {
        this.swipePrompt = this.$t("swipe.waitSwipe");
        if (
          window.cordova &&
          window.cordova.plugins &&
          window.cordova.plugins.LineaPro5
        ) {
          window.cordova.plugins.LineaPro5.setResultCallback(
            this.processSwipeResult
          );
        }
      } else {
        this.inSwipe = false;
      }
    },
    stopSwipe: function() {
      this.inSwipe = false;
      if (
        window.cordova &&
        window.cordova.plugins &&
        window.cordova.plugins.LineaPro5
      ) {
        window.cordova.plugins.LineaPro5.stopSwipe(null);
      }
    },
    swipeCard() {
      if (!this.enableSwipeCard) {
        return;
      }

      console.log("call device");
      this.inSwipe = true;
      this.swipePrompt = this.$t("swipe.connecting");
      if (
        window.cordova &&
        window.cordova.plugins &&
        window.cordova.plugins.LineaPro5
      ) {
        window.cordova.plugins.LineaPro5.startSwipe(this.startSwipeCallback);
      }
    },
    doSearch: function(url) {
      console.log(url);
      this.$utils.showLoading();
      this.$http
        .get(url)
        .then(result => {
          var retData = result.data;
          console.log(retData);
          var message = "";
          try {
            if (retData === undefined || result.status !== 200) {
              message =
                this.$t("common.error") + ": " + retData === undefined
                  ? result.statusText
                  : retData.rspMsg;

              this.$utils.hideLoading();
              this.$utils.toast(message);
              return;
            }

            if (retData.length === 0) {
              this.$utils.hideLoading();
              message = this.$t("enroll.playerNotFound");
              this.$utils.toast(message);
              return;
            }

            this.$utils.hideLoading();
            var playerInfo = retData[0];
            if (playerInfo.IsDiscreet == "true") {
              this.dialogDiscreetPlayerFound = true;
              this.discreetPlayerId = playerInfo.PlayerID;
              this.playerID = "";
              return;
            } else {
              this.dialogDiscreetPlayerFound = false;
              this.discreetPlayerId = "";
            }
            this.playerID = playerInfo.PlayerID;
            this.firstNameHint = playerInfo.FirstName;
            this.lastNameHint = playerInfo.LastName;
          } catch (err) {
            message = this.$t("common.error") + ": " + err;
            this.$utils.hideLoading();
            this.$utils.toast(message);
          }
        })
        .catch(error => {
          var errorMsg = this.$t("common.error") + ": " + error.message;
          this.$utils.hideLoading();
          this.$utils.toast(errorMsg);
        });
    },
    searchPlayerByCardId: function(swipedCardId) {
      this.doSearch("/api/mdb/v1_0/advapi/playerFind?cardid=" + swipedCardId);
    },
    searchPlayerByID() {
      if (this.isBlank(this.playerID)) {
        this.playerID_previous = this.playerID;
        return;
      } else {
        if (this.playerID === this.playerID_previous) {
          return;
        } else {
          this.clearData();
          this.playerID_previous = this.playerID;
        }
      }
      this.doSearch(
        "/api/mdb/v1_0/advapi/playerFind?playerid=" + this.playerID
      );
    },
    clearData() {
      this.currentStep = 1;

      this.firstNameHint = "";
      this.lastNameHint = "";
      this.firstName = "";
      this.middleName = "";
      this.lastName = "";
      this.photoSrc = "/assets/img/photo.png";
      this.idNumber = "";
      this.email = "";
      this.gender = null;
      this.title = null;
      this.agreeProtocol = false;
      this.birthdate = null;
      this.idExpireDate = null;
      this.idState = null;
      this.language = null;
      this.homeAddress = null;
      this.homeCity = null;
      this.homeState = null;
      this.homePostalCode = null;
      this.homePhone = null;
      this.mobilePhone = null;
      this.permitSMS = false;
      this.preferredPhone = null;
      this.mainPhone = null;
      this.preferredComMethod = null;
      this.cameraOriginalPhotoSrc = null;
      this.stepValidate_1 = true;
      this.stepValidate_2 = true;
      this.stepValidate_3 = true;
      this.stepValidate_4 = true;
      this.showSubmit = false;

      this.$refs.cropper.reset();
      this.$refs.signPad.reset();

      var defaultCountryCode = this.$config.get("setting_default_country_code");
      if (defaultCountryCode) {
        this.idCountry = defaultCountryCode;
        this.homeCountry = defaultCountryCode;
      }

      this.idVerified = this.$config.get(
        "setting_identification_expirationdate_default_verified",
        false
      );

      this.$refs.form1.resetValidation();
      this.$refs.form2.resetValidation();
      this.$refs.form3.resetValidation();
      this.$refs.form4.resetValidation();
    },
    stopScan() {
      if (this.inScan == true) {
        if (window.cmbScanner != null) {
          window.cmbScanner.stopScanning();
        }
      }
      this.inScan = false;
    },
    scan() {
      if (window.cmbScanner != null) {
        window.cmbScanner.setPreviewContainerPositionAndSize(0, 25, 100, 50);
        window.cmbScanner
          .loadScanner("DEVICE_TYPE_MOBILE_DEVICE")
          .then(resultLoad => {
            if (resultLoad.result == true) {
              window.cmbScanner.connect().then(resultConn => {
                console.log("connect called back");
                this.inScan = true;
                if (resultConn.status == true) {
                  console.log("connect is true, will set symbol");
                  window.cmbScanner.setSymbologyEnabled(
                    "SYMBOL.PDF417",
                    true,
                    resultSetting => {
                      if (resultSetting.status == true) {
                        console.log("device is ready");
                        window.cmbScanner.setResultCallback(result => {
                          this.inScan = false;
                          console.log("result send");
                          if (
                            result &&
                            result.readString &&
                            result.readString.length > 0
                          ) {
                            var phrased = cmdScanHandler.handleScanDLResult(
                              result.readString
                            );
                            _.assign(this, phrased);
                          }
                        });
                        window.cmbScanner.startScanning(scannerState => {
                          if (scannerState) {
                            this.inScan = true;
                            console.log("in scan -- >" + this.inScan);
                          } else {
                            this.inScan = false;
                            console.log("stoped");
                          }
                        });
                      }
                    }
                  );
                }
              });
            }
          });
      }
    },
    nextStep() {
      this.currentStep = parseInt(this.currentStep);
      if (this.currentStep === this.steps) {
        this.currentStep = 1;
      } else {
        this.currentStep = this.currentStep + 1;
      }
    },
    nextStepValidate1() {
      if (this.$refs.form1.validate()) {
        this.nextStep();
      }
    },
    nextStepValidate2() {
      if (this.$refs.form2.validate()) {
        this.nextStep();
      }
    },
    nextStepValidate3() {
      if (this.$refs.form3.validate()) {
        this.nextStep();
        setTimeout(() => {
          window.dispatchEvent(new Event("resize"));
        }, 2000);
      }
    },
    nextStepValidate4() {
      if (this.$refs.form4.validate() && !this.signatureHasError) {
        this.nextStep();
      }
    },
    UpperFirstName() {
      this.firstName = this.upperCaseFirstLetter(this.firstName);
    },
    UpperMiddleName() {
      this.middleName = this.upperCaseFirstLetter(this.middleName);
    },
    UpperLastName() {
      this.lastName = this.upperCaseFirstLetter(this.lastName);
    },
    UpperCity() {
      this.homeCity = this.upperCaseFirstLetter(this.homeCity);
    },
    TakePhoto() {
      if (window.cordova) {
        var cameraOptions = {
          quality: 75,
          targetHeight: 300,
          targetWidth: 300,
          destinationType: 0, // Camera.DestinationType.DATA_URL
          sourceType: 1, // Camera.PictureSourceType.CAMERA,
          encodingType: 0, // Camera.EncodingType.JPEG,
          mediaType: 0, // Camera.MediaType.PICTURE,
          allowEdit: false,
          correctOrientation: window.$isAndroid
        };

        if (navigator && navigator.camera && navigator.camera.getPicture) {
          navigator.camera.getPicture(
            imageData => {
              this.cameraOriginalPhotoSrc =
                "data:image/jpg;base64," + imageData;
              this.$refs.cropper.reset();
              this.$refs.cropper.replace(this.cameraOriginalPhotoSrc);
              this.photoDialog = true;
            },
            error => {
              console.debug("Unable to obtain picture: " + error, "app");
              if (
                typeof error === "string" &&
                (error.indexOf("cancelled") == -1 &&
                  error.indexOf("No Image Selected") == -1)
              ) {
                var errorMsg = this.$t("common.error") + ": " + error;
                this.$utils.toast(errorMsg);
              }
            },
            cameraOptions
          );
        }
      }
    },
    CropImage() {
      this.photoDialog = false;
      this.photoSrc = this.$refs.cropper
        .getCroppedCanvas()
        .toDataURL("image/jpeg");
    },
    validateStepBySignature() {
      if (this.signatureHasError) {
        this.stepValidate_4 = false;
      } else {
        this.stepValidate_4 = true;
      }
      return true;
    },
    formatDate(date) {
      if (!date) return null;

      const [year, month, day] = date.split("-");
      return `${month}-${day}-${year}`;
    },
    parseDate(date) {
      if (!date) return null;

      const [month, day, year] = date.split("-");
      return `${year}-${month.padStart(2, "0")}-${day.padStart(2, "0")}`;
    },
    checkAge(dateString, minAge) {
      var inmonth = parseInt(dateString.substring(0, 2));
      var indate = parseInt(dateString.substring(3, 5));
      var inyear = parseInt(dateString.substring(6)) - 1900;
      var datestr = new Date();

      if (datestr.getYear() - inyear > minAge) {
        return true;
      } else if (datestr.getYear() - inyear < minAge) {
        return false;
      } else {
        if (datestr.getMonth() + 1 > inmonth) {
          return true;
        } else if (datestr.getMonth() + 1 < inmonth) {
          return false;
        } else {
          if (datestr.getDate() >= indate) {
            return true;
          } else {
            return false;
          }
        }
      }
    },
    upperCaseFirstLetter(val) {
      if (this.isBlank(val)) {
        return val;
      }
      var firstletter = val.charAt(0).toUpperCase();
      return firstletter + val.substring(1);
    },
    isBlank(val) {
      if (val === null) return true;

      if (typeof val === "number") return false;

      if (typeof val === "string") {
        return val.trim().length === 0;
      }

      if (val === undefined) return true;
      if (typeof val === "boolean") return false;

      return true;
    },
    buildPlayerInfo() {
      var playerInfo = {
        PlayerProfile: {
          DuplicateCheck: true,
          DateofBirth: null,
          Gender: null,
          PreferredMethodCommunication: {
            PreferredTypeID: null
          },
          PlayerImage: null,
          PlayerSign: null,
          RedLanternCountryID: null,
          RedLanternProvinceID: null,
          Name: {
            Title: null,
            FirstName: null,
            PreferredName: null,
            MiddleName: "None",
            LastName: null,
            RedLanternFirstName: null,
            RedLanternMiddleName: null,
            RedLanternLastName: null
          },
          Languages: {
            Language: null
          },
          RedLanternAddresses: {
            CompanyName: null,
            PostCode: null,
            Line1: null,
            Line2: null,
            City: null
          },
          Addresses: {
            Address: {
              Address1: null,
              City: null,
              StateProvince: null,
              PostalCode: null,
              Country: null,
              Location: null
            }
          },
          PhoneNumbers: {
            PhoneNumber: []
          },
          Identifications: {
            Identification: {
              Type: null,
              IDNumber: null,
              StateProvince: null,
              Country: null,
              ExpirationDate: null,
              VerificationDate: null
            }
          },
          Emails: {
            Email: {
              Address: null,
              Location: null
            }
          }
        }
      };

      var pp = playerInfo.PlayerProfile;
      // step 1
      pp.Name.FirstName = this.firstName;
      if (!this.isBlank(this.middleName)) {
        pp.Name.MiddleName = this.middleName;
      }
      pp.Name.LastName = this.lastName;

      if (this.photoSrc != "/assets/img/photo.png") {
        pp.PlayerImage = this.photoSrc.substr("data:image/jpeg;base64,".length);
      } else {
        delete pp["PlayerImage"];
      }

      if (!this.isBlank(this.title)) {
        pp.Name.Title = this.title;
      } else {
        delete pp.Name["Title"];
      }
      if (!this.isBlank(this.gender)) {
        pp.Gender = this.gender;
      }
      pp.DateofBirth = this.computedBirthdateFormatted;
      if (!this.isBlank(this.language)) {
        pp.Languages.Language = this.language;
      }

      //step 2
      pp.Identifications.Identification.Type = this.identificationType;
      pp.Identifications.Identification.IDNumber = this.idNumber;
      if (!this.isBlank(this.idState)) {
        pp.Identifications.Identification.StateProvince = this.idState;
      }
      if (!this.isBlank(this.idCountry)) {
        pp.Identifications.Identification.Country = this.idCountry;
      }
      if (!this.isBlank(this.computedIDExpireDateFormatted)) {
        pp.Identifications.Identification.ExpirationDate = this.computedIDExpireDateFormatted;
        if (this.idVerified) {
          var now = new Date();
          pp.Identifications.Identification.VerificationDate = now
            .toISOString()
            .substr(0, 10);
        }
      }

      //step 3
      if (!this.isBlank(this.homeAddress)) {
        pp.Addresses.Address.Address1 = this.homeAddress;
        pp.Addresses.Address.Location = this.addressType;
      }
      if (!this.isBlank(this.homeCountry)) {
        pp.Addresses.Address.Country = this.homeCountry;
      }
      if (!this.isBlank(this.homeCity)) {
        pp.Addresses.Address.City = this.homeCity;
      }
      if (!this.isBlank(this.homeState)) {
        pp.Addresses.Address.StateProvince = this.homeState;
      }
      if (!this.isBlank(this.homePostalCode)) {
        pp.Addresses.Address.PostalCode = this.homePostalCode;
      }

      if (
        this.isBlank(this.homeAddress) &&
        this.isBlank(this.homeCountry) &&
        this.isBlank(this.homeCity) &&
        this.isBlank(this.homeState) &&
        this.isBlank(this.homePostalCode)
      ) {
        delete pp["Addresses"];
      }

      var homePhone = {
        Number: "",
        Location: "Home",
        Preferred: false
      };

      var mobilePhone = {
        Number: "",
        Location: "Mobile",
        Preferred: false,
        PermitTextMsg: "N"
      };

      var mainPhone = {
        Number: "",
        Location: "MainPhone",
        Preferred: false
      };

      if (!this.isBlank(this.homePhone)) {
        homePhone.Number = this.homePhone;
      }

      if (!this.isBlank(this.mobilePhone)) {
        mobilePhone.Number = this.mobilePhone;
        mobilePhone.PermitTextMsg = this.permitSMS ? "Y" : "N";
      }

      if (!this.isBlank(this.mainPhone)) {
        mainPhone.Number = this.mainPhone;
        mainPhone.Location = this.mainPhoneType;
      }

      if (!this.isBlank(this.email)) {
        pp.Emails.Email.Address = this.email;
        pp.Emails.Email.Location = this.emailType;
      } else {
        delete pp["Emails"];
      }

      //step 4
      if (!this.isBlank(this.preferredComMethod)) {
        pp.PreferredMethodCommunication.PreferredTypeID = this.preferredComMethod;
      }

      if (this.preferredPhone === "Home") {
        homePhone.Preferred = true;
      } else if (this.preferredPhone === "Mobile") {
        mobilePhone.Preferred = true;
      } else if (this.preferredPhone === this.mainPhoneType) {
        mainPhone.Preferred = true;
      }

      pp.PhoneNumbers.PhoneNumber.push(homePhone);
      pp.PhoneNumbers.PhoneNumber.push(mobilePhone);
      if (this.showMainPhoneType && !this.isBlank(this.mainPhone)) {
        pp.PhoneNumbers.PhoneNumber.push(mainPhone);
      }

      // please keep the variable name
      const { isEmpty, data } = this.$refs.signPad.saveSignature();
      if (!isEmpty) {
        pp.PlayerSign = data.substr("data:image/jpeg;base64,".length);
      } else {
        delete pp["PlayerSign"];
      }

      return playerInfo;
    },
    submit() {
      this.showSubmit = true;
      if (this.signatureMandatory) {
        this.validateStepBySignature();
      }

      this.stepValidate_1 = this.$refs.form1.validate();
      this.stepValidate_2 = this.$refs.form2.validate();
      this.stepValidate_3 = this.$refs.form3.validate();
      this.stepValidate_4 =
        this.$refs.form4.validate() && !this.signatureHasError;

      if (
        this.stepValidate_1 &&
        this.stepValidate_2 &&
        this.stepValidate_3 &&
        this.stepValidate_4 &&
        !this.signatureHasError
      ) {
        this.dialogRequireEmail =
          this.email.trim() === "" && this.enrollRequireEmail;

        if (!this.dialogRequireEmail) this.submitPlayerInfo(true);
      }

      if (!this.stepValidate_1) {
        this.currentStep = 1;
      } else if (!this.stepValidate_2) {
        this.currentStep = 2;
      } else if (!this.stepValidate_3) {
        this.currentStep = 3;
      } else if (!this.stepValidate_4) {
        this.currentStep = 4;
      }
    },
    submitPlayerInfo(dupCheck) {
      this.$utils.showLoading();
      var playerInfo = this.buildPlayerInfo();
      playerInfo.PlayerProfile.DuplicateCheck = dupCheck;

      var url = "";
      if (this.convertTempPlayer) {
        playerInfo.PlayerProfile.PlayerID = this.playerID;
        url = "/api/player/update";
      } else {
        url = "/api/player/enroll";
      }

      this.$http
        .post(url, playerInfo)
        .then(result => {
          console.info(result);
          var retData = result.data;
          var message = "";
          try {
            if (retData === undefined || result.status !== 200) {
              message =
                this.$t("common.error") + ": " + retData === undefined
                  ? result.statusText
                  : retData.rspMsg;

              this.$utils.hideLoading();
              this.$utils.toast(message);
              return;
            }

            if (!retData.Success) {
              this.$utils.hideLoading();
              if (
                retData.ErroMessage === "FOUND_DUPLICATE" &&
                retData.DuplicateCheck > 0
              ) {
                this.dialogFoundDuplicate = true;
                return;
              }

              message = this.$t("common.error") + ": " + retData.ErroMessage;
              this.$utils.toast(message);
              return;
            }

            this.$utils.hideLoading();
            var userName = this.firstName + " " + this.lastName;
            this.$router.replace({
              name: "enrollSuccess",
              params: {
                id: retData.PlayerID,
                name: userName,
                idNumber: this.idNumber,
                convertMode: this.convertTempPlayer
              }
            });
          } catch (err) {
            message = this.$t("common.error") + ": " + err;
            this.$utils.hideLoading();
            this.$utils.toast(message);
          }
        })
        .catch(error => {
          var errorMsg = this.$t("common.error") + ": " + error.message;
          if (
            error.response != undefined &&
            error.response.data != undefined &&
            error.response.data.ErroMessage != undefined &&
            error.response.data.ErroMessage.length > 0
          ) {
            errorMsg = error.response.data.ErroMessage;
          }
          this.$utils.hideLoading();
          this.$utils.toast(errorMsg);
        });

      return;
    },
    saveBirthday(date) {
      this.$refs.birthdayMenu.save(date);
      this.birthdate = this.birthdateInit;
    },
    saveIDExpireDate(date) {
      this.$refs.idExpireDateMenu.save(date);
    }
  },
  beforeMount: function() {
    this.genders = [
      { value: "M", text: this.$t("enroll.male") },
      { value: "F", text: this.$t("enroll.female") },
      { value: "", text: this.$t("enroll.notSpecified") }
    ];
    this.submitButtonName = this.$t("common.submit");
    this.pageTitle = this.$t("enroll.enrollPlayer");

    var minAge = this.$config.get("setting_player_min_age", 21);
    this.maxBirthDate = moment({
      hour: 0,
      minute: 0,
      seconds: 0,
      milliseconds: 0
    })
      .subtract(minAge, "y")
      .format("YYYY-MM-DD");
    this.birthdateInit = this.maxBirthDate;
    var maxBirthdateRule = v =>
      (v && this.checkAge(v, minAge)) || "Player must be older than " + minAge;
    this.birthdateRules.push(maxBirthdateRule);

    this.RegistrationProtocol = this.$config.get(
      "setting_registration_protocol"
    );

    this.enableDuplicateEnroll = this.$config.get(
      "setting_enable_duplicate_enroll"
    );

    if (this.enableDuplicateEnroll) {
      this.duplicatePromptMsg = this.$t("enroll.duplicate_found");
      this.duplicateConfirmMsg = this.$t("common.no");
    } else {
      this.duplicatePromptMsg = this.$t("enroll.duplicate_no_overwrite");
      this.duplicateConfirmMsg = this.$t("common.close");
    }

    var mainPhoneType = this.$config.get("setting_main_phone_type");
    var defaultPhoneTypes = directEnrollCache.getPreferredPhones();

    var targetPhoneTypes = defaultPhoneTypes.slice(0);

    if (
      mainPhoneType.length > 0 &&
      mainPhoneType !== "Home" &&
      mainPhoneType !== "Mobile"
    ) {
      var lastPhoneType = targetPhoneTypes.pop();
      targetPhoneTypes.push({
        Abbrev: mainPhoneType,
        Description: mainPhoneType
      });
      targetPhoneTypes.push(lastPhoneType);
      this.showMainPhoneType = true;
    }

    this.preferredPhones = targetPhoneTypes;
    this.mainPhoneType = mainPhoneType;
    this.mainPhoneTypeDesc = mainPhoneType + " " + this.$t("common.phone");

    this.regProtocolMandatory = this.$config.get(
      "setting_registration_protocol_mandatory",
      true
    );
    this.signatureMandatory = this.$config.get(
      "setting_signature_mandatory",
      true
    );
    this.enrollRequireEmail = this.$config.get(
      "setting_enroll_require_email",
      true
    );

    var defaultCountryCode = this.$config.get("setting_default_country_code");
    if (defaultCountryCode) {
      this.idCountry = defaultCountryCode;
      this.homeCountry = defaultCountryCode;
    }

    this.idVerified = this.$config.get(
      "setting_identification_expirationdate_default_verified",
      false
    );

    this.enableSwipeCard =
      window.$isIOS && this.$config.get("setting_linea_pro5_enabled", false);

    this.swipCardIcon = this.enableSwipeCard ? "credit_card" : null;
  },
  updated: function() {
    if (parseInt(this.currentStep) == 4) {
      window.dispatchEvent(new Event("resize"));
    }
  },
  activated: function() {
    this.clearData();
    if (window.$isAndroid) {
      document.addEventListener("backbutton", this.onBackKeyDown, false);
    }
  },
  deactivated: function() {
    if (window.$isAndroid) {
      document.removeEventListener("backbutton", this.onBackKeyDown, false);
    }
  },
  beforeRouteEnter(to, from, next) {
    directEnrollCache
      .initialize()
      .then(() => {
        next();
      })
      .catch(() =>
        alert(
          "Unable to initialize the languages, country, state or communication method."
        )
      );
  }
};
</script>

<style scoped>
.v-stepper__header {
  height: 28px;
}
.stepper_header_icon >>> .v-stepper__step__step {
  height: 20px;
  width: 20px;
  min-width: 20px;
}
.stepper_header_icon >>> .v-stepper__step__step {
  font-size: 10pt;
}
.v-stepper__content {
  padding: 16px 24px 8px 24px;
}
.textarea-icon {
  font-size: 16pt;
}
.protocol_checkbox >>> label.v-label {
  font-size: 9pt;
}
.mr-0 >>> .v-input--selection-controls__input {
  margin-right: 0px;
}
.mr-1 >>> .v-input--selection-controls__input {
  margin-right: 0px;
}
.idExpVerify >>> .v-input--selection-controls__input {
  margin-right: 0px;
}
.container {
  padding-left: 0;
  padding-right: 0;
}
@media only screen and (max-width: 959px) {
  .container {
    padding-left: 0;
    padding-right: 0;
  }
}
.permitsms_checkbox >>> label.v-label {
  font-size: 14px;
}
.permitsms_checkbox >>> .v-input--selection-controls__input {
  margin-right: 0px;
}
pre {
  white-space: pre-wrap /* Since CSS 2.1 */;
  white-space: -moz-pre-wrap /* Mozilla, since 1999 */;
  white-space: -pre-wrap /* Opera 4-6 */;
  white-space: -o-pre-wrap /* Opera 7 */;
  word-wrap: break-word; /* Internet Explorer 5.5+ */
}
.slide-fade-enter-active {
  transition: all 0.5s ease;
}
.slide-fade-enter {
  transform: translateX(10px);
  opacity: 0;
}
</style>
