import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { agroindustrias } from '../api/agroindustriasAPI';
import TemplateDetalhesAgroindustria from '../components/templates/TemplateDetalhesAgroindustria';
import { Spin, message } from 'antd';

const DetalhesAgroindustria = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [dados, setDados] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDetalhes = async () => {
      try {
        const resposta = await agroindustrias.detalhesAgroindustria(id);
        setDados(resposta);
      } catch (err) {
        message.error('Erro ao carregar detalhes da agroindústria.');
        navigate('/agroindustrias', { replace: true });
      } finally {
        setLoading(false);
      }
    };
    fetchDetalhes();
  }, [id, navigate]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <Spin size="large" />
      </div>
    );
  }

  return <TemplateDetalhesAgroindustria dados={dados} />;
};

export default DetalhesAgroindustria;
