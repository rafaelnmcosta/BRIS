import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CardUsuario from '../components/CardUsuario';
import HeadbarSistema from '../components/HeadbarSistema';

const ListaUsuarios = () => {
  const [usuarios, setUsuarios] = useState([]);

  const usuariosTeste = [
    { id: 1, nome: 'João', email: 'joao@email.com', tipo: '2'},
    { id: 2, nome: 'Maria', email: 'maria@email.com', tipo: '1'},
    { id: 3, nome: 'Pedro', email: 'pedro@email.com', tipo: '1'},
    { id: 4, nome: 'Jorge', email: 'jorge@email.com', tipo: '3'},
  ];

  useEffect(() => {
    /*
    axios.get('https://api.exemplo.com/usuarios')
      .then(response => {
        setUsuarios(response.data);
      })
      .catch(error => {
        console.error('Erro ao buscar os dados dos usuarios:', error);
      });
    */
   setUsuarios(usuariosTeste);
  }, []);

  const handleEdit = (usuarioId) => {
    // Lógica para editar os dados do usuario com o ID fornecido
    console.log('Editar dados do usuário com ID:', usuarioId);
  };

  return (
    <div>
        <HeadbarSistema />
        <div className='page-content'>
            <a href='/'> {'< '} Voltar</a>
            <h2>Lista de usuários</h2>
            {usuarios.map(usuario => (
                <div key={usuario.id} style={{ marginBottom: 16 }}>
                    <CardUsuario
                    id={usuario.id}
                    nome={usuario.nome}
                    email={usuario.email}
                    tipo={usuario.tipo}
                    onEdit={() => handleEdit(usuario.id)}
                    />
                </div>
            ))}
        </div>
    </div>
  );
};

export default ListaUsuarios;
