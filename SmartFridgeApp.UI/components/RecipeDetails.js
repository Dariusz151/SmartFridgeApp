import React, { useEffect, useState } from "react";
import {
  StyleSheet,
  View,
  TouchableHighlight,
  Modal,
  Text,
} from "react-native";

export default function RecipeDetails({ navigation }) {
  const [dataLoading, finishLoading] = useState(true);
  const [recipesData, setData] = useState([]);
  const [modalVisible, setModalVisible] = useState(false);

  <View style={styles.centeredView}>
    <Modal
      animationType="slide"
      transparent={true}
      visible={modalVisible}
      onRequestClose={() => {
        Alert.alert("Modal has been closed.");
      }}
    >
      <View style={styles.centeredView}>
        <View style={styles.modalView}>
          <Text style={styles.modalText}>Hello World!</Text>

          <TouchableHighlight
            style={{ ...styles.openButton, backgroundColor: "#2196F3" }}
            onPress={() => {
              setModalVisible(!modalVisible);
            }}
          >
            <Text style={styles.textStyle}>Hide</Text>
          </TouchableHighlight>
        </View>
      </View>
    </Modal>

    <TouchableHighlight
      style={styles.openButton}
      onPress={() => {
        setModalVisible(true);
      }}
    >
      <Text style={styles.textStyle}>Details</Text>
    </TouchableHighlight>
  </View>;
}

const styles = StyleSheet.create({
  centeredView: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    marginTop: 22,
  },
  modalView: {
    margin: 20,
    backgroundColor: "white",
    borderRadius: 20,
    padding: 35,
    alignItems: "center",
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  openButton: {
    backgroundColor: "#F194FF",
    borderRadius: 20,
    padding: 10,
    elevation: 2,
  },
  textStyle: {
    color: "white",
    fontWeight: "bold",
    textAlign: "center",
  },
  modalText: {
    marginBottom: 15,
    textAlign: "center",
  },
});
