import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateCadastroAgroindustria from '../components/templates/TemplateCadastroAgroindustria';
import { agroindustrias } from '../api/agroindustriasAPI';

const CadastroAgroindustria = () => {
  const navigate = useNavigate();
  const [erros, setErros] = useState({});

  const handleSubmit = async (formData) => {
    const novosErros = {};

    if (!formData.razaoSocial) novosErros.razaoSocial = 'Razão Social é obrigatória';
    if (!formData.nomeFantasia) novosErros.nomeFantasia = 'Nome Fantasia é obrigatório';
    if (!formData.cnpj) novosErros.cnpj = 'CNPJ é obrigatório';

    if (Object.keys(novosErros).length > 0) {
      setErros(novosErros);
      return;
    }

    try {
      await agroindustrias.cadastrarAgroindustria({
        razaoSocial: formData.razaoSocial,
        nomeFantasia: formData.nomeFantasia,
        cnpj: formData.cnpj
      });
      navigate('/agroindustrias');
    } catch (error) {
      console.error('Erro ao cadastrar agroindústria:', error);
    }
  };

  return (
    <TemplateCadastroAgroindustria
      onSubmit={handleSubmit}
      erros={erros}
    />
  );
};

export default CadastroAgroindustria;
