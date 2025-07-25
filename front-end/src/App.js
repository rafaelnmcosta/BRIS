import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

// imports de componentes de funcionamento
import LoggedRoute from './components/serviceComponents/LoggedRoute';
import AuthenticatedRoute from './components/serviceComponents/AuthenticatedRoute';
import { AuthProvider, useAuth } from './services/AuthContext';
import { NotificationProvider } from './services/NotificationContext';
import { ValidationProvider } from './services/ValidationContext';

// header e outros comuns a todas as paginas
import Header from './components/organisms/Header';
import BotaoVoltarGlobal from './components/atoms/BotaoVoltarGlobal';

// imports de paginas
import Login from './pages/Login';
import ListaVinculos from './pages/ListaVinculos';
import Home from './pages/Home';
import Teste from './pages/Teste';

import TabelaUsuarios from './pages/TabelaUsuarios';
import TabelaUsuariosInativos from './pages/TabelaUsuariosInativos';
import CadastroUsuario from './pages/CadastroUsuario';
import AtivarUsuario from './pages/AtivarUsuario';
import EdicaoUsuario from './pages/EdicaoUsuario';

import Perfil from './pages/Perfil';
import EdicaoPerfil from './pages/EdicaoPerfil';

import TabelaAgroindustrias from './pages/TabelaAgroindustrias';
import TabelaAgroindustriasInativas from './pages/TabelaAgroindustriasInativas';
import CadastroAgroindustria from './pages/CadastroAgroindustria';
import EdicaoAgroindustria from './pages/EdicaoAgroindustria';
import DetalhesAgroindustria from './pages/DetalhesAgroindustria';

import TabelaGranjas from './pages/TabelaGranjas';


import { Spin } from 'antd';

function AppContent() {
  const { loading } = useAuth();

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <Spin size="large" tip="Carregando dados do usuário..." />
      </div>
    );
  }

  return (
    <>
      <Header />
      <BotaoVoltarGlobal />
      <Routes>
        {/* Rotas públicas */}
        <Route path="/login" element={<Login />} />
        <Route path="/teste" element={<CadastroUsuario />} />

        {/* Rotas protegidas por isLogged */}
        <Route element={<LoggedRoute />}>
          <Route path="/vinculos" element={<ListaVinculos />} />
        </Route>

        {/* Rotas protegidas por isAuthenticated */}
        <Route element={<AuthenticatedRoute />}>
          <Route path="/home" element={<Home />} />
          <Route path="/teste" element={<Teste />} />
          <Route path="/usuarios" element={<TabelaUsuarios />} />
          <Route path="/usuarios/inativos" element={<TabelaUsuariosInativos />} />
          <Route path="/usuarios/cadastrar" element={<CadastroUsuario />} />
          <Route path="/usuarios/reativar/:id" element={<AtivarUsuario />} />
          <Route path="/usuarios/:id/editar" element={<EdicaoUsuario />} />
          <Route path="/perfil" element={<Perfil />} />
          <Route path="/perfil/editar" element={<EdicaoPerfil />} />
          <Route path="/agroindustrias" element={<TabelaAgroindustrias />} />
          <Route path="/agroindustrias/inativas" element={<TabelaAgroindustriasInativas />} />
          <Route path="/agroindustrias/cadastrar" element={<CadastroAgroindustria />} />
          <Route path="/agroindustrias/:id" element={<DetalhesAgroindustria />} />
          <Route path="/agroindustrias/:id/editar" element={<EdicaoAgroindustria />} />
          <Route path="/granjas" element={<TabelaGranjas />} />
        </Route>
      </Routes>
    </>
  );
}

function App() {
  return (
    <Router>
      <NotificationProvider>
        <AuthProvider>
          <ValidationProvider>
            <AppContent />
          </ValidationProvider>
        </AuthProvider>
      </NotificationProvider>
    </Router>
  );
}

export default App;