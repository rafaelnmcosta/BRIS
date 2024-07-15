import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Login from './pages/Login';
import Cadastro from './pages/Cadastro';

const App = () => {
  return (
    <BrowserRouter>
        <Routes>
          <Route exact path="/" Component={Home} />
          <Route path="/sobre" Component={Sobre} />
          <Route path="/login" Component={Login} />
          <Route path="/cadastro" Component={Cadastro} />
        </Routes>
    </BrowserRouter>
  );
};

export default App;
