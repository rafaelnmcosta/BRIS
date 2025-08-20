// imports de bibliotecas externas
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Spin } from 'antd';

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
import TabelaGranjasInativas from './pages/TabelaGranjasInativas';
import CadastroGranja from './pages/CadastroGranja';
import EdicaoGranja from './pages/EdicaoGranja';
import DetalhesGranja from './pages/DetalhesGranja';

import TabelaAnimais from './pages/TabelaAnimais';
import TabelaAnimaisInativos from './pages/TabelaAnimaisInativos';
import CadastroAnimal from './pages/CadastroAnimal';
import EdicaoAnimal from './pages/EdicaoAnimal';
import DetalhesAnimal from './pages/DetalhesAnimal';

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
          {/* Rotas de navegação */}
          <Route path="/home" element={<Home />} />
          <Route path="/teste" element={<Teste />} />
          {/* Rotas de usuários */}
          <Route path="/usuarios" element={<TabelaUsuarios />} />
          <Route path="/usuarios/inativos" element={<TabelaUsuariosInativos />} />
          <Route path="/usuarios/cadastrar" element={<CadastroUsuario />} />
          <Route path="/usuarios/reativar/:id" element={<AtivarUsuario />} />
          <Route path="/usuarios/:id/editar" element={<EdicaoUsuario />} />
          {/* Rotas de perfil */}
          <Route path="/perfil" element={<Perfil />} />
          <Route path="/perfil/editar" element={<EdicaoPerfil />} />
          {/* Rotas de agroindústrias */}
          <Route path="/agroindustrias" element={<TabelaAgroindustrias />} />
          <Route path="/agroindustrias/inativas" element={<TabelaAgroindustriasInativas />} />
          <Route path="/agroindustrias/cadastrar" element={<CadastroAgroindustria />} />
          <Route path="/agroindustrias/:id" element={<DetalhesAgroindustria />} />
          <Route path="/agroindustrias/:id/editar" element={<EdicaoAgroindustria />} />
          {/* Rotas de granjas */}
          <Route path="/granjas" element={<TabelaGranjas />} />
          <Route path="/granjas/inativas" element={<TabelaGranjasInativas />} />
          <Route path="/granjas/cadastrar" element={<CadastroGranja />} />
          <Route path="/granjas/:id" element={<DetalhesGranja />} />
          <Route path="/granjas/:id/editar" element={<EdicaoGranja />} />
          {/* Rotas de animais */}
          <Route path="/animais" element={<TabelaAnimais />} />
          <Route path="/animais/inativos" element={<TabelaAnimaisInativos />} />
          <Route path="/animais/cadastrar" element={<CadastroAnimal />} />
          <Route path="/animais/:id" element={<DetalhesAnimal />} />
          <Route path="/animais/:id/editar" element={<EdicaoAnimal />} />
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