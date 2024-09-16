import React, { useState, useEffect } from 'react';
import CardUsuarioPendente from '../components/CardUsuarioPendente';
import HeadbarSistema from '../components/HeadbarSistema';
import api from '../services/api';

import './Listas.css';

const ListaUsuariosPendentes = () => {
  const [usuariosPendentes, setUsuariosPendentes] = useState([]);

  useEffect(() => {
    const fetchUsuariosPendentes = async () => {
      try {
        const response = await api.get('http://localhost:5206/api/Usuarios/usuarios/ativar');
        setUsuariosPendentes(response.data);
      } catch (error) {
        console.error('Erro ao buscar os Usuarios Pendentes:', error);
      }
    };

    fetchUsuariosPendentes();
  }, []);

  return (
    <div>
        <HeadbarSistema />
        <div className='page-content'>
            <a href='/usuarios'> {'< '} Voltar</a>
            <h2 className='title'>Lista de usuários ainda não ativados</h2>
            {usuariosPendentes.map(usuario => (
                <div key={usuario.id} style={{ marginBottom: 16 }}>
                    <CardUsuarioPendente
                    id={usuario.id}
                    nome={usuario.nome}
                    email={usuario.email}
                    />
                </div>
            ))}
        </div>
    </div>
  );
};

export default ListaUsuariosPendentes;
