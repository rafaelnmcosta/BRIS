import React, { useState, useEffect } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { IdcardOutlined, UserOutlined } from '@ant-design/icons';

const FormEdicaoAgroindustria = ({ onSubmit, erros = {}, initialData = {} }) => {
  const [formData, setFormData] = useState({
    razaoSocial: '',
    nomeFantasia: '',
    cnpj: ''
  });

  useEffect(() => {
    if (initialData) {
      setFormData({
        razaoSocial: initialData.razaoSocial || '',
        nomeFantasia: initialData.nomeFantasia || '',
        cnpj: initialData.cnpj || ''
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ ...formData, id: initialData.id });
  };

  return (
    <form className="w-full" onSubmit={handleSubmit}>
      <InputSemBordaComLabel
        label="Razão Social"
        name="razaoSocial"
        value={formData.razaoSocial}
        onChange={handleChange}
        placeholder="Razão Social"
        icone={<UserOutlined className="text-green-dark" />}
        erro={erros.razaoSocial}
      />

      <InputSemBordaComLabel
        label="Nome Fantasia"
        name="nomeFantasia"
        value={formData.nomeFantasia}
        onChange={handleChange}
        placeholder="Nome Fantasia"
        icone={<UserOutlined className="text-green-dark" />}
        erro={erros.nomeFantasia}
      />

      <InputSemBordaComLabel
        label="CNPJ"
        name="cnpj"
        value={formData.cnpj}
        onChange={handleChange}
        placeholder="XX.XXX.XXX/0001-XX"
        icone={<IdcardOutlined className="text-green-dark" />}
        erro={erros.cnpj}
        mask="99.999.999/9999-99"
      />

      <div className="w-1/2 mx-auto mt-8">
        <BotaoPrimario texto="Salvar Alterações" type="submit" />
      </div>
    </form>
  );
};

export default FormEdicaoAgroindustria;
