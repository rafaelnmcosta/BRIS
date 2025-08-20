import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { granjas } from '../api/granjasAPI';
import TemplateDetalhesGranja from '../components/templates/TemplateDetalhesGranja';
import { Spin, message } from 'antd';

const DetalhesGranja = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [dados, setDados] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDetalhes = async () => {
      try {
        const resposta = await granjas.detalhesGranja(id);
        setDados(resposta);
      } catch (err) {
        message.error('Erro ao carregar detalhes da granja.');
        navigate('/granjas', { replace: true });
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

  return <TemplateDetalhesGranja dados={dados} />;
};

export default DetalhesGranja;
