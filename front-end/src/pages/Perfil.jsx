import React, { useEffect, useState } from 'react';
import { perfil } from '../api/perfilAPI';
import TemplatePerfil from '../components/templates/TemplatePerfil';
import { Spin, message } from 'antd';

const Perfil = () => {
  const [dadosPerfil, setDadosPerfil] = useState(null);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    const buscarPerfil = async () => {
      try {
        const dados = await perfil.acessarPerfil();
        setDadosPerfil(dados);
      } catch (erro) {
        message.error('Erro ao carregar os dados do perfil.');
      } finally {
        setCarregando(false);
      }
    };

    buscarPerfil();
  }, []);

  if (carregando) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spin size="large" />
      </div>
    );
  }

  return <TemplatePerfil dadosPerfil={dadosPerfil} />;
};

export default Perfil;
