import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import ProtectedRoute from './components/ProtectedRoute';

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
import ListaUsuariosPendentes from './pages/ListaUsuariosPendentes';
import Usuario from './pages/Usuario';
import EditarUsuario from './pages/EditarUsuario';
import Perfil from './pages/Perfil';
import EditarPerfil from './pages/EditarPerfil';
import CadastrarUsuario from './pages/CadastrarUsuario';
// import ListaGranjas from './pages/Granjas';

const App = () => {
  return (
    <BrowserRouter>
        <Routes>
          <Route path="/" element={<ProtectedRoute element={<Home />} />} />
          <Route path="/sobre" element={<Sobre />} />
          <Route path="/login" element={<Login />} />
          <Route path="/cadastro" element={<Cadastro />} />
          <Route path="/nova-dose" element={<ProtectedRoute element={<NovaDose />} />} />
          <Route path="/animais" element={<ProtectedRoute element={<ListaAnimais />} />} />
          <Route path="/animais/cadastrar" element={<ProtectedRoute element={<CadastrarAnimal />} />} />
          <Route path="/animais/:id" element={<ProtectedRoute element={<Animal />} />} />
          <Route path="/animais/:id/editar" element={<ProtectedRoute element={<EditarAnimal />} />} />
          <Route path="/animais/:id/nova-dose" element={<ProtectedRoute element={<Amostra />} />} />
          <Route path="/usuarios" element={<ProtectedRoute element={<ListaUsuarios />} />} />
          <Route path="/usuarios/ativar" element={<ProtectedRoute element={<ListaUsuariosPendentes />} />} />
          <Route path="/usuarios/cadastrar" element={<ProtectedRoute element={<CadastrarUsuario />} />} />
          <Route path="/usuarios/:id" element={<ProtectedRoute element={<Usuario />} />} />
          <Route path="/usuarios/:id/editar" element={<ProtectedRoute element={<EditarUsuario />} />} />
          <Route path="/perfil/:id" element={<ProtectedRoute element={<Perfil />} />} />
          <Route path="/perfil/:id/editar" element={<ProtectedRoute element={<EditarPerfil />} />} />
          {/* <Route path="/granjas" element={<ListaGranjas />} /> */}
        </Routes>
    </BrowserRouter>
  );
};

export default App;
