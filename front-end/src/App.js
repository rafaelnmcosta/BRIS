import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Login from './pages/Login';

const App = () => {
  return (
    <BrowserRouter>
        <Routes>
          <Route exact path="/" Component={Login} />
          <Route path="/sobre" Component={Sobre} />
          <Route path="/home" Component={Home} />
        </Routes>
    </BrowserRouter>
  );
};

export default App;
