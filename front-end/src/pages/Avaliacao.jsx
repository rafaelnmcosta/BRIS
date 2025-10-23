import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { message, Spin } from "antd";
import TemplateAvaliacao from "../components/templates/TemplateAvaliacao";
import { avaliacoes } from "../api/avaliacoesAPI";

const Avaliacao = () => {
    const { id } = useParams();
    const [avaliacao, setAvaliacao] = useState(null);
    const [loading, setLoading] = useState(true);

    const carregarAvaliacao = async () => {
        try {
            setLoading(true);
            const dados = await avaliacoes.detalhes(id);
            setAvaliacao(dados);
        } catch {
            message.error("Erro ao carregar avaliação.");
        } finally {
            setLoading(false);
        }
    };

    const interromper = async () => {
        try {
            await avaliacoes.interromper(id);
            message.success("Avaliação interrompida com sucesso!");
            carregarAvaliacao();
        } catch {
            message.error("Erro ao interromper avaliação.");
        }
    };

    const reativar = async () => {
        try {
            await avaliacoes.reativar(id);
            message.success("Avaliação reativada com sucesso!");
            carregarAvaliacao();
        } catch {
            message.error("Erro ao reativar avaliação.");
        }
    };

    const finalizar = async () => {
        try {
            await avaliacoes.finalizar(id);
            message.success("Avaliação finalizada com sucesso!");
            carregarAvaliacao();
        } catch {
            message.error("Erro ao finalizar avaliação.");
        }
    };

    useEffect(() => {
        if (id) carregarAvaliacao();
    }, [id]); // recarrega quando id muda

    if (loading) return <Spin size="large" className="flex justify-center" />;
    if (!avaliacao) return <p>Erro ao carregar avaliação.</p>;

    return (
        <TemplateAvaliacao
            avaliacao={avaliacao}
            onAtualizar={carregarAvaliacao}
            onInterromper={interromper}
            onReativar={reativar}
            onFinalizar={finalizar}
        />
    );
};

export default Avaliacao;