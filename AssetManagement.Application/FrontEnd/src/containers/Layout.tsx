import { Layout } from "react-admin";
import React from "react";
import MenuBar from "../components/sidebar/SidebarMenu";
// import AppBar from '../components/appBar';

const App = (props) => (
  <Layout {...props} menu={MenuBar} /* appBar={AppBar} */ />
);

export default App;
