import { Layout } from "react-admin";
import React from "react";
import MenuBar from "../components/sidebar/SidebarMenu";
import Header from '../components/header/Header';

const App = (props) => (
  <Layout {...props} menu={MenuBar} appBar={Header} toggleSidebar={false} />
);

export default App;
