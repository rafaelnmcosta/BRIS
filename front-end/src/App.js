import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Login from './pages/Login';
import Cadastro from './pages/Cadastro';
import NovaDose from './pages/NovaDose';
import ListaAnimais from './pages/ListaAnimais';
import Animal from './pages/Animal';
import EditarAnimal from './pages/EditarAnimal';
import CadastrarAnimal from './pages/CadastrarAnimal';
import Amostra from './pages/NovaAmostra';
import ListaUsuarios from './pages/ListaUsuarios';
import Usuario from './pages/Usuario';
import EditarUsuario from './pages/EditarUsuario';
import Perfil from './pages/Perfil';
import EditarPerfil from './pages/EditarPerfil';
import CadastrarUsuario from './pages/CadastrarUsuario';
//import ListaGranjas from './pages/Granjas';

const App = () => {
  return (
    <BrowserRouter>
        <Routes>
          <Route exact path="/" Component={Home} />
          <Route path="/sobre" Component={Sobre} />
          <Route path="/login" Component={Login} />
          <Route path="/cadastro" Component={Cadastro} />
          <Route path="/nova-dose" Component={NovaDose} />
          <Route path="/animais" Component={ListaAnimais} />
          <Route path="/animais/cadastrar" Component={CadastrarAnimal} />
          <Route path="/animais/:id" Component={Animal} />
          <Route path="/animais/:id/editar" Component={EditarAnimal} />
          <Route path="/animais/:id/nova-dose" Component={Amostra} />
          <Route path="/usuarios" Component={ListaUsuarios} />
          <Route path="/usuarios/cadastrar" Component={CadastrarUsuario} />
          <Route path="/usuarios/:id" Component={Usuario} />
          <Route path="/usuarios/:id/editar" Component={EditarUsuario} />
          <Route path="/perfil/:id" Component={Perfil} />
          <Route path="/perfil/:id/editar" Component={EditarPerfil} />
          {/*
          <Route path="/granjas" Component={ListaGranjas} />
          */}
        </Routes>
    </BrowserRouter>
  );
};

export default App;
