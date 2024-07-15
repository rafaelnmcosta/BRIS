import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Login from './pages/Login';
import Cadastro from './pages/Cadastro';
import NovaAmostra from './pages/NovaAmostra';
import ListaAnimais from './pages/Animais';
import ListaUsuarios from './pages/Usuarios';
//import ListaGranjas from './pages/Granjas';

const App = () => {
  return (
    <BrowserRouter>
        <Routes>
          <Route exact path="/" Component={Home} />
          <Route path="/sobre" Component={Sobre} />
          <Route path="/login" Component={Login} />
          <Route path="/cadastro" Component={Cadastro} />
          <Route path="/animais" Component={ListaAnimais} />
          <Route path="/nova-amostra" Component={NovaAmostra} />
          <Route path="/usuarios" Component={ListaUsuarios} />
          {/*
          <Route path="/granjas" Component={ListaGranjas} />
          */}
        </Routes>
    </BrowserRouter>
  );
};

export default App;
